using System;
using System.Collections.Generic;
using System.Web.Http;
using Domain;
using Domain.Authentication;
using Domain.CareProvider;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api/claim")]
    public class ClaimsController : ApiController
    {
        private readonly IEventSourcedRepository<CareProvider> careProviderEventRepository;
        private readonly ClaimDraftRepository claimDrafts;

        public ClaimsController(IEventSourcedRepository<CareProvider> careProviderEventRepository, ClaimDraftRepository claimDrafts)
        {
            this.careProviderEventRepository = careProviderEventRepository;
            this.claimDrafts = claimDrafts;
        }

        [Route(""), HttpGet]
        public IEnumerable<ClaimDraft> GetAll()
        {
            return claimDrafts.GetDrafts();
        }

        [Route("{claimId}"), HttpGet]
        public ClaimDraft Get(Guid claimId)
        {
            return claimDrafts.GetDraft(claimId);
        }

        [Route("patient/{patientId}"), HttpPost]
        public Guid Create(Guid patientId, Visit visit)
        {
            var provider = careProviderEventRepository.CurrentProvider(ActionContext.ActionArguments);

            var command = new CareProvider.StartClaim
            {
                PatientId = patientId,
                Visit = visit
            };

            command.ApplyTo(provider);

            careProviderEventRepository.Save(provider);

            return provider.Id;
        }

        [Route(""), HttpPut]
        public void Update(Guid draftId, Visit visit)
        {
            var provider = careProviderEventRepository.CurrentProvider(ActionContext.ActionArguments);

            var command = new CareProvider.UpdateClaimDraft
            {
                ClaimDraftId = draftId,
                Visit = visit
            };

            command.ApplyTo(provider);

            careProviderEventRepository.Save(provider);
        }
    }
}
