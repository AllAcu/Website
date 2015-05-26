using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class CompletedVerificationDetails
    {
        public Guid VerificationId { get; set; }

        public Benefits Benefits { get; set; }
    }

    public class CompletedVerificationDetailsEventHandler
        : IUpdateProjectionWhen<CareProvider.VerificationApproved>
    {
        private readonly AllAcuSiteDbContext dbContext;
        public CompletedVerificationDetailsEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.VerificationApproved @event)
        {
            dbContext.ApprovedVerifications.Add(new CompletedVerificationDetails
            {
                VerificationId = @event.VerificationId,
            });
        }
    }
}
