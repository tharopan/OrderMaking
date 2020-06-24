using OrderMaking.Business;
using OrderMaking.Models;
using System.Reflection.Emit;
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

        public void Delete(string barcode)
        {
            labelAppService.Delete(barcode);
        }

        public void Delete()
        {
            labelAppService.Delete();
        }
    }
}