using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Sql;

namespace Domain
{
    public class ClaimDraftWorking : 
        IUpdateProjectionWhen<ClaimFilingProcess.ClaimInitiated>,
        IUpdateProjectionWhen<ClaimFilingProcess.ClaimUpdated>

    {
        private readonly ClaimDraftRepository repository;

        public ClaimDraftWorking(ClaimDraftRepository repository)
        {
            this.repository = repository;
        }

        public void UpdateProjection(ClaimFilingProcess.ClaimInitiated @event)
        {
            repository.StartDraft(@event.Claim);
        }

        public void UpdateProjection(ClaimFilingProcess.ClaimUpdated @event)
        {
            repository.Update(@event.Claim);
        }
    }
}
