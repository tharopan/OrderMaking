using OrderMaking.Business;
using OrderMaking.Models;
using System.Web.Http;

namespace OrderMaking.API.Controllers
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

        public void Delete(string barcode)
        {
            //cartAppService.Remove(shoppingCart);
        }
    }
}
