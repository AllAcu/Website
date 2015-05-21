using System;
using System.Linq;
using Domain.CareProvider;
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
        IUpdateProjectionWhen<CareProvider.VerificationDraftCreated>,
        IUpdateProjectionWhen<CareProvider.VerificationDraftUpdated>,
        IUpdateProjectionWhen<CareProvider.VerificationRequestSubmitted>
    {
        private AllAcuSiteDbContext dbContext;
        public PendingVerificationRequestHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.VerificationDraftCreated @event)
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

        public void UpdateProjection(CareProvider.VerificationDraftUpdated @event)
        {
            var request = dbContext.VerificationRequestDrafts.First(v => v.VerificationId == @event.VerificationId);

            request.Request = @event.Request;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.VerificationRequestSubmitted @event)
        {
            var request = dbContext.VerificationRequestDrafts.First(v => v.VerificationId == @event.VerificationId);
            dbContext.VerificationRequestDrafts.Remove(request);
            dbContext.SaveChanges();
        }
    }
}
