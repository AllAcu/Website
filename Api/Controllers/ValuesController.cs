﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain;
using Domain.Repository;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Sql;

namespace Api.Controllers
{
    //    [Authorize]
    public class ValuesController : ApiController
    {
        private static ClaimFilingProcess process = new ClaimFilingProcess();
        private readonly IEventSourcedRepository<ClaimFilingProcess> draftEvents;
        //private readonly SqlEventSourcedRepository<ClaimDraft> repository;
        private readonly ClaimDraftRepository Claims;

        public ValuesController(IEventSourcedRepository<ClaimFilingProcess> draftEvents, ClaimDraftRepository claims)
        {
            this.draftEvents = draftEvents;
            this.Claims = claims;
        }

        //public ValuesController()
        //{
        //    this.repository = new SqlEventSourcedRepository<ClaimDraft>();
        //    var context = this.repository.GetEventStoreContext();
        //}

        [Route("make"), HttpGet]
        public Guid Make(string name)
        {
            var claim = new ClaimFilingProcess();

            claim.EnactCommand(new ClaimFilingProcess.StartClaim
            {
                Claim = new ClaimDraft
                {
                    Patient = name,
                    Diagnosis = "Bum knee"
                }
            });

            draftEvents.Save(claim);

            return claim.Id;
        }

        [Route("claims"), HttpGet]
        public IEnumerable<ClaimDraft> GetAll()
        {
            return Claims.GetDrafts();
        }

        [Route(""), HttpGet]
        public ClaimDraft Get(Guid id)
        {
            Debug.WriteLine("Git it");

            var draft = draftEvents.GetLatest(id);
            return draft.Claim;
        }

        [Route("punch"), HttpGet]
        public void Punch(Guid id)
        {
            Debug.WriteLine("Punches");

            var draft = draftEvents.GetLatest(id);
            draft.EnactCommand(new ClaimFilingProcess.Approve());
        }
    }
}
