using OrderMaking.Business;
using System;
using System.Web.Http;

namespace OrderMaking.ConsoleApp
{
    public class FunctionController : ApiController
    {
        GenerateOrderSheet orderSheetAppService;

        public FunctionController()
        {
            orderSheetAppService = new GenerateOrderSheet();
        }

        public void Post()
        {
            orderSheetAppService.GenerateOrder();
        }
    }
}