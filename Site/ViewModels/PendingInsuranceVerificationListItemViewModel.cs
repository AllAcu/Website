﻿using System;
using System.Linq;
using Domain.CareProvider;
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
        IUpdateProjectionWhen<CareProvider.VerificationDraftCreated>,
        IUpdateProjectionWhen<CareProvider.VerificationDraftUpdated>,
        IUpdateProjectionWhen<CareProvider.VerificationRequestSubmitted>,
        IUpdateProjectionWhen<CareProvider.VerificationApproved>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.VerificationApproved @event)
        {
            var verification = dbContext.VerificationList.Find(@event.VerificationId);
            verification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.VerificationDraftCreated @event)
        {
            dbContext.VerificationList.Add(new PendingInsuranceVerificationListItemViewModel
            {
                VerificationId = @event.VerificationId,
                PatientId = @event.PatientId,
                Patient = dbContext.PatientDetails.Find(@event.PatientId)?.Name,
                Status = "Draft"
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.VerificationDraftUpdated @event)
        {
            var verification = dbContext.VerificationList.Find(@event.VerificationId);
            verification.Provider = @event.Request.Provider;
            verification.Comments = @event.Request.Comments;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.VerificationRequestSubmitted @event)
        {
            var verification = dbContext.VerificationList.Find(@event.VerificationId);
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
    }
}
