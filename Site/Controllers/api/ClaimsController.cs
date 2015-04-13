using System;
using System.Collections.Generic;
using System.Web.Http;
using Domain.CareProvider;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
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

        [Route(""), HttpPost]
        public Guid Create(ClaimDraft draft)
        {
            var provider = careProviderEventRepository.GetLatest(CareProvider.HardCodedId);

            provider.EnactCommand(new CareProvider.StartClaim
            {
                Claim = draft
            });

            careProviderEventRepository.Save(provider);

            return provider.Id;
        }

        [Route(""), HttpPut]
        public void Update(ClaimDraft draft)
        {
            var claim = careProviderEventRepository.GetLatest(draft.Id);

            claim.EnactCommand(new CareProvider.UpdateClaim
            {
                Claim = draft
            });

            careProviderEventRepository.Save(claim);
        }
    }
}
