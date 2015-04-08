using Domain.Repository;
using Microsoft.Its.Domain;

namespace Domain
{
    public class ClaimDraftWorking : 
        IUpdateProjectionWhen<ClaimFilingProcess.ClaimInitiated>,
        IUpdateProjectionWhen<ClaimFilingProcess.ClaimUpdated>,
        IUpdateProjectionWhen<ClaimFilingProcess.Submitted>

    {
        private readonly ClaimDraftRepository draftRepository;

        public ClaimDraftWorking(ClaimDraftRepository draftRepository)
        {
            this.draftRepository = draftRepository;
        }

        public void UpdateProjection(ClaimFilingProcess.ClaimInitiated @event)
        {
            draftRepository.StartDraft(@event.Claim);
        }

        public void UpdateProjection(ClaimFilingProcess.ClaimUpdated @event)
        {
            draftRepository.Update(@event.Claim);
        }

        public void UpdateProjection(ClaimFilingProcess.Submitted @event)
        {
            var draft = draftRepository.GetDraft(@event.AggregateId);
            draftRepository.Submit(draft);
        }
    }
}
