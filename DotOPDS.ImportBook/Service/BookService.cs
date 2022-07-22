using AsbtCore.UtilsV2;
using DotOPDS.Database;
using DotOPDS.Database.Models;
using DotOPDS.ImportBook.Utils;
using DotOPDS.Shared;
using DotOPDS.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace DotOPDS.ImportBook.Service
{

    public interface IBookService
    {
        void DeleteBeforeSendDublicate();
        void DeleteDublicate();
        void FillDatabase();
    }

    public class BookService : IBookService
    {
        private readonly MyDbContext db;
        private readonly IConfiguration conf;
        private readonly PresentationOptions options;
        private readonly LuceneIndexStorage textSearch;

        public BookService(IConfiguration conf, IOptions<PresentationOptions> options, LuceneIndexStorage textSearch, MyDbContext db)
        {
            this.conf = conf;
            this.options = options.Value;
            this.textSearch = textSearch;
            this.db = db;
        }


        /// <summary>
        /// Локал папкада crc_list.json асосланиб дубликатларни учириш
        /// </summary>
        public void DeleteBeforeSendDublicate()
        {
            var path = conf["Presentation:Path"];

            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles();

            Dictionary<string, string> CrcList;
            Dictionary<string, string> LocalCrcList = new Dictionary<string, string>();
            if (File.Exists(path + "\\crc_list.json"))
            {
                var ss = File.ReadAllText(path + "\\crc_list.json");
                CrcList = ss.FromJson<Dictionary<string, string>>();
                List<string> list = new List<string>(CrcList.Keys);

                foreach (var it in list)
                {
                    try
                    {
                        var fi = new FileInfo(it);
                        if (File.Exists(path + "\\" + fi.Name))
                        {
                            Console.WriteLine(path + "\\" + fi.Name);
                            File.Delete(path + "\\" + fi.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }


                List<string> vlist = new List<string>(CrcList.Values);
                foreach (var it in vlist)
                {
                    try
                    {

                        for (var j = 0; j < files.Length; j++)
                        {
                            string s1;
                            if (LocalCrcList.ContainsKey(files[j].FullName))
                                s1 = LocalCrcList[files[j].FullName];
                            else
                            {
                                s1 = CCRC32.GetCRC32File(files[j].FullName);
                                LocalCrcList.Add(files[j].FullName, s1);
                            }

                            if (it == s1)
                            {
                                Console.WriteLine(files[j].FullName);
                                File.Delete(files[j].FullName);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public void DeleteDublicate()
        {
            var path = conf["Presentation:Path"];

            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles();

            Dictionary<string, string> CrcList;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "crc_list.json"))
            {
                var ss = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "crc_list.json");
                CrcList = ss.FromJson<Dictionary<string, string>>();
            }
            else
                CrcList = new Dictionary<string, string>();

            for (var i = 0; i < files.Length; i++)
            {
                for (var j = i + 1; j < files.Length; j++)
                {
                    try
                    {
                        Console.Title = $"Ind: {i} - {files.Length}";

                        var s1 = "";
                        var s2 = "";

                        if (CrcList.ContainsKey(files[i].FullName))
                            s1 = CrcList[files[i].FullName];
                        else
                        {
                            s1 = CCRC32.GetCRC32File(files[i].FullName);
                            CrcList.Add(files[i].FullName, s1);
                        }

                        if (CrcList.ContainsKey(files[j].FullName))
                            s2 = CrcList[files[j].FullName];
                        else
                        {
                            s2 = CCRC32.GetCRC32File(files[j].FullName);
                            CrcList.Add(files[j].FullName, s2);
                        }

                        if (s1 == s2)
                        {
                            Console.WriteLine(files[j].Name);
                            File.Delete(files[j].FullName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            var sj = CrcList.ToJson();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "crc_list.json", sj);
        }

        public void FillDatabase()
        {
            string[] spearator = { "_", " ", "-", ".", ",", "(", ")" };

            textSearch.Cleanup();
            textSearch.Open();

            var path = conf["Presentation:Path"];

            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles();
            var defaultImagePath = AppDomain.CurrentDomain.BaseDirectory + "Images" + Path.DirectorySeparatorChar + "defaultImage.png";
            var defaultImage = System.IO.File.ReadAllBytes(defaultImagePath);

            var tran = db.Database.BeginTransaction();
            var LibraryIdGuid = Guid.NewGuid();
            byte[] thumbnail;

            try
            {
                for (var i = 0; i < files.Length; i++)
                {
                    Console.Title = $"Ind: {i} - {files.Length}";

                    var image = new Thumbnail();
                    thumbnail = defaultImage;
                    image.Data = defaultImage;

                    if (files[i].Extension == ".pdf")
                    {
                        thumbnail = ThumbnailProvider.ExtractImagesFromPDF(files[i].FullName);
                        if (thumbnail is not null)
                            image.Data = thumbnail;
                    }

                    db.Thumbnails.Add(image);
                    db.SaveChanges();

                    var book = new Book();
                    book.Name = files[i].Name;
                    book.FileName = files[i].FullName;
                    book.Authors = null;
                    book.Genres = new int[] { 1 };

                    var strlist = files[i].Name.Split(spearator, StringSplitOptions.RemoveEmptyEntries);
                    book.BookTags = new int[strlist.Length];

                    var taglist = new List<int>();

                    for (var j = 0; j < strlist.Length; j++)
                    {
                        if (strlist[j].Length == 1 || int.TryParse(strlist[j], out _) || strlist[j] == files[i].Extension[1..] && strlist[j].Length < 1) continue;


                        var id = db.BookTags.Where(x => x.Name == strlist[j].ToLower()).Select(x => x.Id).FirstOrDefault();

                        if (id == 0)
                        {
                            var tags = new BookTag { Name = strlist[j].ToLower() };
                            db.BookTags.Add(tags);
                            db.SaveChanges();

                            id = tags.Id;
                        }

                        taglist.Add(id);
                    }

                    book.BookTags = taglist.ToArray();
                    book.ImageId = image.Id;

                    db.Books.Add(book);
                    db.SaveChanges();

                    textSearch.Insert(new Contract.Models.Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = files[i].Name,
                        Date = DateTime.Now,
                        File = files[i].FullName,
                        Ext = files[i].Extension,
                        Annotation = "",
                        SeriesNo = 1,
                        UpdatedFromFile = false,
                        UpdatedAt = DateTime.Now,
                        LibraryId = LibraryIdGuid,
                        Cover = new Contract.Models.Cover { Data = thumbnail == null ? defaultImage : thumbnail, ContentType = "image/png" },
                        Genres = new Contract.Models.Genre[] { new Contract.Models.Genre() { Name = "Разработка" } }
                    });
                }
                tran.Commit();

                textSearch.Dispose();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
