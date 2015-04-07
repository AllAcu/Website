using System.Web.Mvc;

namespace AllAcu.Controllers
{
    public class ClaimsController : Controller
    {
        public ActionResult Index()
        {
            return View("Claims");
        }
    }
}
