using System;
using System.Linq;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class PendingInsuranceVerificationListItemViewModel
    {
        public Guid VerificationId { get; set; }
        public Guid PatientId { get; set; }
        public string Patient { get; set; }
        public string Status { get; set; }
        public string Provider { get; set; }
        public string Comments { get; set; }
    }

    public class InsuranceVerificationViewModelHandler :
        IUpdateProjectionWhen<InsuranceVerification.VerificationStarted>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationDraftUpdated>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationRequestSubmitted>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationApproved>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationRevised>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(InsuranceVerification.VerificationApproved @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationStarted @event)
        {
            dbContext.VerificationList.Add(new PendingInsuranceVerificationListItemViewModel
            {
                VerificationId = @event.AggregateId,
                PatientId = @event.PatientId,
                Patient = dbContext.PatientDetails.Find(@event.PatientId)?.Name,
                Status = "Draft"
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationDraftUpdated @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Provider = @event.Request.Provider;
            verification.Comments = @event.Request.Comments;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationRequestSubmitted @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Status = "Submitted";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            if (string.IsNullOrEmpty(@event.UpdatedName)) return;

            var verifications = dbContext.VerificationList.Where(v => v.PatientId == @event.PatientId);
            foreach (var verification in verifications)
            {
                verification.Patient = @event.UpdatedName;
            }

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationRevised @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Status = "Draft";

            dbContext.SaveChanges();
        }
    }
}
