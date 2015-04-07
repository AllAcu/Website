using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Domain;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [RoutePrefix("api/claim")]
    public class ClaimsController : ApiController
    {
        private readonly IEventSourcedRepository<ClaimFilingProcess> draftEvents;
        private readonly ClaimDraftRepository Claims;

        public ClaimsController(IEventSourcedRepository<ClaimFilingProcess> draftEvents, ClaimDraftRepository claims)
        {
            this.draftEvents = draftEvents;
            Claims = claims;
        }

        [Route(""), HttpGet]
        public IEnumerable<ClaimDraft> GetAll()
        {
            return Claims.GetDrafts();
        }

        [Route("{claimId}"), HttpGet]
        public ClaimDraft Get(Guid claimId)
        {
            Debug.WriteLine("Git it");

            var draft = Claims.GetDraft(claimId);
            return draft;
        }

        [Route(""), HttpPost]
        public Guid Create(ClaimDraft draft)
        {
            var claim = new ClaimFilingProcess();

            claim.EnactCommand(new ClaimFilingProcess.StartClaim
            {
                Claim = draft
            });

            draftEvents.Save(claim);

            return claim.Id;
        }

        [Route(""), HttpPut]
        public void Update(ClaimDraft draft)
        {
            var claim = draftEvents.GetLatest(draft.Id);

            claim.EnactCommand(new ClaimFilingProcess.UpdateClaim
            {
                Claim = draft
            });

            draftEvents.Save(claim);
        }

        [Route("{claimId}/approve"), HttpPost]
        public void Approve(Guid id)
        {
            Debug.WriteLine("Approving");

            var draft = draftEvents.GetLatest(id);
            draft.EnactCommand(new ClaimFilingProcess.Approve());
        }
    }
}
