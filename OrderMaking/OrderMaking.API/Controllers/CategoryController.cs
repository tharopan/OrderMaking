using OrderMaking.Business;
using OrderMaking.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace OrderMaking.API.Controllers
{
    public class CategoryController : ApiController
    {
        CategoryAppService categoryAppService;

        public CategoryController()
        {
            categoryAppService = new CategoryAppService();
        }

        public IList<DeprecatedCategory> Get()
        {
            return categoryAppService.Get();
        }
    }
}