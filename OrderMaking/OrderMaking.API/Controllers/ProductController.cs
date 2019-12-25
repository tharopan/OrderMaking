using OrderMaking.Business;
using OrderMaking.Models;
using System;
using System.Web.Http;

namespace OrderMaking.API.Controllers
{
    public class ProductController : ApiController
    {
        DeprecatedProductAppService productAppService;

        public ProductController()
        {
            productAppService = new DeprecatedProductAppService();
        }

        // GET: Product
        [HttpGet]
        public DeprecatedProduct Get(string barCode)
        {
            return productAppService.Get(barCode);
        }
    }
}