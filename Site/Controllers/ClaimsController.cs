using System.Web.Mvc;

namespace Api.Controllers
{
    public class ClaimsController : Controller
    {
        public ActionResult Index(string[] anything)
        {
            return View("Claims");
        }
    }
}
