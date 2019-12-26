using OrderMaking.Business;
using OrderMaking.Models;
using System.Web.Http;

namespace OrderMaking.ConsoleApp
{
    public class CartController : ApiController
    {
        CartAppService cartAppService;

        public CartController()
        {
            cartAppService = new CartAppService();
        }

        public void Post(ShoppingCart shoppingCart)
        {
            cartAppService.Add(shoppingCart);
        }
    }
}
