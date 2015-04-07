using System.Web.Mvc;

namespace Api.Controllers
{
    public class ClaimsController : Controller
    {
        public ActionResult Index()
        {
            return View("Claims");
        }
    }
}
