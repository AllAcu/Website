﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Microsoft.Its.Domain;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventSourcedRepository<ClaimFilingProcess> store;

        public HomeController(IEventSourcedRepository<ClaimFilingProcess> store)
        {
            this.store = store;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

//            store.GetLatest()

            return View();
        }
    }
}