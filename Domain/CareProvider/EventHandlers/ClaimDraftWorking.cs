using Domain.Repository;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public class ClaimDraftWorking : 
        IUpdateProjectionWhen<CareProvider.ClaimStarted>,
        IUpdateProjectionWhen<CareProvider.ClaimUpdated>

    {
        private readonly ClaimDraftRepository draftRepository;

        public ClaimDraftWorking(ClaimDraftRepository draftRepository)
        {
            this.draftRepository = draftRepository;
        }

        public void UpdateProjection(CareProvider.ClaimStarted @event)
        {
            draftRepository.StartDraft(@event.Claim);
        }

        public void UpdateProjection(CareProvider.ClaimUpdated @event)
        {
            draftRepository.Update(@event.Claim);
        }

        //public void UpdateProjection(CareProvider.Submitted @event)
        //{
        //    var draft = draftRepository.GetDraft(@event.AggregateId);
        //    draftRepository.Submit(draft);
        //}
    }
}
