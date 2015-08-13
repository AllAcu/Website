using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class PendingInsuranceVerificationListItemViewModel
    {
        public Guid VerificationId { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public string Status { get; set; }

        public virtual UserDetails AssignedTo { get; set; }
    }

    public class InsuranceVerificationViewModelHandler :
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Started>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.DraftUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.RequestSubmitted>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Approved>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Rejected>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Approved @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Started @event)
        {
            dbContext.VerificationList.Add(new PendingInsuranceVerificationListItemViewModel
            {
                VerificationId = @event.AggregateId,
                PatientId = @event.PatientId,
                PatientName = dbContext.PatientDetails.Find(@event.PatientId)?.Name,
                Status = "Draft"
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.DraftUpdated @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.RequestSubmitted @event)
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
                verification.PatientName = @event.UpdatedName;
            }

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Rejected @event)
        {
            var verification = dbContext.VerificationList.Find(@event.AggregateId);
            verification.Status = "Draft";

            dbContext.SaveChanges();
        }
    }
}
