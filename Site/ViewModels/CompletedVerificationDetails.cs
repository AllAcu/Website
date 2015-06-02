using System;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class CompletedVerificationDetails
    {
        public Guid VerificationId { get; set; }
        public Benefits Benefits { get; set; }
    }

    public class CompletedVerificationDetailsEventHandler
        : IUpdateProjectionWhen<InsuranceVerification.VerificationApproved>
    {
        private readonly AllAcuSiteDbContext dbContext;
        public CompletedVerificationDetailsEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(InsuranceVerification.VerificationApproved @event)
        {
            dbContext.ApprovedVerifications.Add(new CompletedVerificationDetails
            {
                VerificationId = @event.VerificationId
            });
        }
    }
}
