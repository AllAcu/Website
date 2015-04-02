using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain;
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

        public ValuesController(IEventSourcedRepository<ClaimFilingProcess> draftEvents)
        {
            this.draftEvents = draftEvents;
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
                    Patient = new Patient { Name = name },
                    Diagnosis = "Bum knee"
                }
            });

            draftEvents.Save(claim);

            return claim.Id;
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
