using DotOPDS.ImportBook.Service;

namespace BookNameFix
{
    public class Program
    {
        private readonly IBookService service;

        public Program(IBookService service)
        {
            this.service = service;
        }


        public int Run()
        {

            service.DeleteDublicate();
            service.FillDatabase();

            // service.DeleteBeforeSendDublicate();

            return 0;
        }
    }
}





