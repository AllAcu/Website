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
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationStarted>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationDraftUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationRequestSubmitted>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationApproved>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationRevised>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationApproved @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationStarted @event)
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

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationDraftUpdated @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Provider = @event.Request.Provider;
            verification.Comments = @event.Request.Comments;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationRequestSubmitted @event)
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

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationRevised @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Status = "Draft";

            dbContext.SaveChanges();
        }
    }
}
