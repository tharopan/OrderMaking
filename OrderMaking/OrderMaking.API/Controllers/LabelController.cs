using OrderMaking.Business;
using OrderMaking.Models;
using System.Web.Http;

namespace OrderMaking.API.Controllers
{
    public class LabelController : ApiController
    {
        LabelAppService labelAppService;

        public LabelController()
        {
            labelAppService = new LabelAppService();
        }

        public void Post(LabelItem lableItem)
        {
            labelAppService.Add(lableItem);
        }
    }
}