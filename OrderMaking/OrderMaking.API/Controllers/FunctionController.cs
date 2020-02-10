using OrderMaking.Business;
using OrderMaking.Models;
using System;
using System.Web.Http;

namespace OrderMaking.API.Controllers
{
    public class FunctionController : ApiController
    {
        GenerateOrderSheet orderSheetAppService;

        public FunctionController()
        {
            orderSheetAppService = new GenerateOrderSheet();
        }

        public void Post(OrderType obj)
        {
            if(obj != null)
            {
                if(!string.IsNullOrEmpty(obj.OrderList) && obj.OrderList == "Cigarettes")
                {
                    orderSheetAppService.GenerateCigarettes();
                }
            }
            else
            {
                orderSheetAppService.GenerateOrder();
            }
        }
    }
}