using OrderMaking.Business;
using System.Web.Http;
using System.Web.Mvc;

namespace OrderMaking.API.Controllers
{
    public class PrintFunctionController : ApiController
    {
        LabelAppService labelApp;

        public PrintFunctionController()
        {
            labelApp = new LabelAppService();
        }

        public void Post()
        {
            labelApp.PrintLable();
        }
    }
}