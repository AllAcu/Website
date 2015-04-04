using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Api.Models;
using Domain.Repository;

namespace Api.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ClaimDraftRepository claims = AllAcuWebApplication.Container.Resolve<ClaimDraftRepository>();

        // GET: Claims
        public ActionResult Index()
        {
            var drafts = claims.GetDrafts();

            return View(new ClaimsIndex
            {
                Claims = drafts.ToArray()
            });
        }

        // GET: Claims/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Claims/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

    }
}
