using OrderMaking.Business;
using System.Web.Http;
using System.Web.Mvc;

namespace OrderMaking.API.Controllers
{
    public class SyncController : ApiController
    {
        InternalAppService internalAppService;

        public SyncController()
        {
            internalAppService = new InternalAppService();
        }

        public void Post()
        {
            internalAppService.SyncDb();
        }
    }
}