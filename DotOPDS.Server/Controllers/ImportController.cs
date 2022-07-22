//using System.Threading.Tasks;
//using DotOPDS.ImportBook.Service;
//using Microsoft.AspNetCore.Mvc;

//namespace DotOPDS.Server.Controllers;

//[Route("books")]
//public class ImportController : ControllerBase
//{

//    private readonly IBookService service;

//    public ImportController(IBookService service)
//    {
//        this.service = service;
//    }

//    [Route("import", Name = nameof(ImBook))]
//    public async Task<string> ImBook()
//    {
//        await service.FillDatabase();
//        return "OK";
//    }

//    [Route("fix", Name = nameof(FixNameBook))]
//    public async Task<string> FixNameBook()
//    {
//        await service.DeleteDublicate();
//        return "OK";
//    }

//}
