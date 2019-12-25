using OrderMaking.Business;
using OrderMaking.Models;
using System;
using System.Web.Http;

namespace OrderMaking.API.Controllers
{
    public class ProductController : ApiController
    {
        ProductAppService productAppService;

        public ProductController()
        {
            productAppService = new ProductAppService();
        }

        // GET: Product
        public Product Get(string barCode)
        {
            return productAppService.Get(barCode);
        }
    }
}