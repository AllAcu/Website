using System.Web.Mvc;

namespace Api.Controllers
{
    public class ClaimsController : Controller
    {
        // GET: Claims
        public ActionResult Index()
        {
            return View("Claims");
        }
    }
}
