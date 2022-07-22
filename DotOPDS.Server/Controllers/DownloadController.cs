using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotOPDS.Contract.Models;
using DotOPDS.Extensions;
using DotOPDS.Server.Services;
using DotOPDS.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotOPDS.Server.Controllers;

[Route("download")]
public class DownloadController : ControllerBase
{
    private readonly ILogger<DownloadController> _logger;
    private readonly LuceneIndexStorage _textSearch;
    private readonly MimeHelper _mimeHelper;

    public DownloadController(
        ILogger<DownloadController> logger,
        LuceneIndexStorage textSearch,
        MimeHelper mimeHelper)
    {
        _logger = logger;
        _textSearch = textSearch;
        _mimeHelper = mimeHelper;
    }

    [Route("file/{id:guid}/{ext}", Name = nameof(GetFile))]
    public async Task<IActionResult> GetFile(Guid id, string ext, CancellationToken cancellationToken)
    {
        var book = GetBookById(id);


        if (System.IO.File.Exists(book.File))
        {
            var content = System.IO.File.OpenRead(book.File);
            return File(content, _mimeHelper.GetContentType(ext), GetBookSafeName(book, ext));
        }
        else return NotFound();
    }

    [Route("cover/{id:guid}", Name = nameof(GetCover))]
    public IActionResult GetCover(Guid id)
    {
        var book = GetBookById(id);

        if (book.Cover == null || book.Cover.Data == null || book.Cover.ContentType == null)
        {
            _logger.LogWarning("No cover found for file {Id}", id);
            return NotFound();
        }

        return File(book.Cover.Data, book.Cover.ContentType);
    }

    private Book GetBookById(Guid id)
    {
        var books = _textSearch.SearchExact(out var total, "guid", id.ToString(), take: 1);
        if (total != 1)
        {
            _logger.LogDebug("File {Id} not found", id);
            throw new KeyNotFoundException("Key Not Found: " + id);
        }

        _logger.LogDebug("File {Id} found in {Time}ms", id, _textSearch.Time);
        return books.First();
    }

    private static readonly Dictionary<char, string> _dangerChars = new()
    {
        { '/', "" },
        { '\\', "" },
        { ':', "" },
        { '*', "" },
        { '?', "" },
        { '"', "'" },
        { '<', "«" },
        { '>', "»" },
        { '|', "" },
    };

    private static string FilterDangerChars(string s)
    {
        var res = "";
        foreach (var c in s)
        {
            if (_dangerChars.ContainsKey(c)) res += _dangerChars[c];
            else res += c;
        }
        return res;
    }

    private static string GetBookSafeName(Book book, string ext)
    {
        var result = book.Title!;
        if (book.Authors != null)
        {
            var firstAuthor = book.Authors.FirstOrDefault();
            if (firstAuthor != default)
            {
                result = $"{firstAuthor.GetScreenName()} - {result}";
            }
        }
        return $"{FilterDangerChars(result)}.{ext}";
    }
}
