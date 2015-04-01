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
        private static ClaimDraft draft = new ClaimDraft();
        private readonly IEventSourcedRepository<ClaimDraft> repository;
        //private readonly SqlEventSourcedRepository<ClaimDraft> repository;

        public ValuesController(IEventSourcedRepository<ClaimDraft> repository)
        {
            this.repository = repository;
        }

        //public ValuesController()
        //{
        //    this.repository = new SqlEventSourcedRepository<ClaimDraft>();
        //    var context = this.repository.GetEventStoreContext();
        //}

        [Route("make"), HttpGet]
        public Guid Make(string name)
        {
            var claim = new ClaimDraft();

            claim.EnactCommand(new ClaimDraft.Create
            {
                Diagnosis = "Bum knee"
            });

            repository.Save(claim);

            return claim.Id;
        }

        [Route("punch"), HttpGet]
        public void Punch(Guid id)
        {
            Debug.WriteLine("Punches");

            var draft = repository.GetLatest(id);
            draft.EnactCommand(new ClaimDraft.Approve());
        }
    }
}
