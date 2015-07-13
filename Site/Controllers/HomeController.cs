using System.Web.Mvc;

namespace AllAcu.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "AllAcu Home";

            return View();
        }
    }
}
