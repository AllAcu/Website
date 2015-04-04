using System.Web.Mvc;
using Domain.Repository;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        private Domain.Repository.ClaimDraftRepository claims = AllAcuWebApplication.Container.Resolve<ClaimDraftRepository>();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Pages";

            var drafts = claims.GetDrafts();
            ViewBag.Drafts = drafts;

            return View();
        }
    }
}
