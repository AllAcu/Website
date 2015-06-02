using System;
using System.Linq;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class PendingVerificationRequest
    {
        public Guid VerificationId { get; set; }
        public Guid PatientId { get; set; }
        public string Status { get; set; }
        public VerificationRequest Request { get; set; }
    }

    public class PendingVerificationRequestHandler :
        IUpdateProjectionWhen<InsuranceVerification.VerificationStarted>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationDraftUpdated>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationRequestSubmitted>
    {
        private AllAcuSiteDbContext dbContext;
        public PendingVerificationRequestHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(InsuranceVerification.VerificationStarted @event)
        {
            var request = new PendingVerificationRequest
            {
                PatientId = @event.PatientId,
                VerificationId = @event.VerificationId,
                Request = new VerificationRequest(),
                Status = "Draft"
            };

            dbContext.VerificationRequestDrafts.Add(request);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationDraftUpdated @event)
        {
            var request = dbContext.VerificationRequestDrafts.First(v => v.VerificationId == @event.VerificationId);

            request.Request = @event.Request;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationRequestSubmitted @event)
        {
            var request = dbContext.VerificationRequestDrafts.First(v => v.VerificationId == @event.VerificationId);
            dbContext.VerificationRequestDrafts.Remove(request);
            dbContext.SaveChanges();
        }
    }
}
