using System;
using System.Threading;
using System.Threading.Tasks;
using DotOPDS.Contract.Models;
using DotOPDS.Shared;
using DotOPDS.Shared.Plugins;
using Microsoft.Extensions.Logging;

namespace DotOPDS.Database.Services;

public class BookParsersPool
{
    private volatile LuceneIndexStorage _lucene;
    private readonly ILogger<BookParsersPool> _logger;
    private readonly FileUtils _fileUtils;
    private readonly PluginProvider _pluginProvider;

    public BookParsersPool(ILogger<BookParsersPool> logger,
        LuceneIndexStorage lucene,
        FileUtils fileUtils,
        PluginProvider pluginProvider)
    {
        _logger = logger;
        _lucene = lucene;
        _fileUtils = fileUtils;
        _pluginProvider = pluginProvider;
    }

    public async Task Update(Book book, CancellationToken cancellationToken)
    {
        if (book.UpdatedFromFile || book.Ext == null) return;
        _logger.LogDebug("Book being updated, id:{Id}", book.Id);

        var parser = _pluginProvider.GetFileFormatReader(book.Ext);

        if (parser == null) return;

        var replace = false;
        try
        {
            using (var stream = _fileUtils.GetBookFile(book))
            {
                replace = await parser.ReadAsync(book, stream, cancellationToken);
            }
            _logger.LogDebug("Book updated successfully, id:{Id}", book.Id);
        }
        catch (Exception e)
        {
            _logger.LogDebug("Book update failed, id:{Id}. {ErrorMessage}", book.Id, e.Message);
        }

        if (replace)
        {
            book.UpdatedFromFile = true;
            _lucene.Replace(book);
        }
    }
}
