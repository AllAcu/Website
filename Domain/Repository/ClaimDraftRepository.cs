using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Repository
{
    public class ClaimDraftRepository
    {
        private readonly ClaimsReadModelDbContext context;

        public ClaimDraftRepository(ClaimsReadModelDbContext context)
        {
            this.context = context;
        }

        public void StartDraft(ClaimDraft draft)
        {
            context.Drafts.Add(draft);
            context.SaveChanges();
        }

        public IEnumerable<ClaimDraft> GetDrafts()
        {
            return context.Drafts.Take(10);
        }

        public ClaimDraft GetDraft(Guid id)
        {
            return context.Drafts.Find(id);
        }

        public void Update(ClaimDraft updated)
        {
            var draft = context.Drafts.Find(updated.Id);

            draft.Visit.Diagnosis = updated.Visit.Diagnosis;
            draft.Visit.DateOfService = updated.Visit.DateOfService;

            context.SaveChanges();
        }

        public void Submit(ClaimDraft claim)
        {
            context.Drafts.Remove(claim);
            context.SubmittedClaims.Add(new SubmittedClaim
            {
                Id = claim.Id,
                Patient = claim.Patient,
                Diagnosis = claim.Visit.Diagnosis,
                DateOfService = claim.Visit.DateOfService
            });
            context.SaveChanges();
        }

        public void Remove(ClaimDraft draft)
        {
            context.Drafts.Remove(draft);
            context.SaveChanges();
        }
    }
}
