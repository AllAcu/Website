using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain;
using Microsoft.Its.Domain;

namespace Api.Controllers
{
//    [Authorize]
    public class ValuesController : ApiController
    {
        private static ClaimDraft draft = new ClaimDraft();
        private readonly IEventSourcedRepository<ClaimDraft> repository;

        public ValuesController(IEventSourcedRepository<ClaimDraft> repository)
        {
            this.repository = repository;
        }

        [Route("make")]
        public Guid Make(string name)
        {
            var claim = new ClaimDraft();
            repository.Save(claim);

            return claim.Id;
        }

        [Route("punch"), HttpGet]
        public void Punch(Guid id)
        {
            Debug.WriteLine("Punches");

            repository.GetLatest(id).EnactCommand(new ClaimDraft.Approve());
        }
    }
}
