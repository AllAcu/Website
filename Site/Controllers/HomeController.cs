using System.Web.Mvc;
using Domain.Repository;

namespace AllAcu.Controllers
{
    public class HomeController : Controller
    {
        private ClaimDraftRepository claims = AllAcuWebApplication.Container.Resolve<ClaimDraftRepository>();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Pages";

            var drafts = claims.GetDrafts();
            ViewBag.Drafts = drafts;

            return View();
        }
    }
}
