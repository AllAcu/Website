using System;
using System.Collections.Generic;
using Domain.Biller;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class BillerDetails
    {
        public static Guid AllAcuBillerId;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual IList<BillerRole> Users { get; set; } = new List<BillerRole>();
    }

    public class AllAcuBillerHandler
        : IUpdateProjectionWhen<Biller.BillerInitialized>
    {
        private readonly AllAcuSiteDbContext dbContext;
        public AllAcuBillerHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public void UpdateProjection(Biller.BillerInitialized @event)
        {
            dbContext.Billers.Add(new BillerDetails
            {
                Id = @event.AggregateId,
                Name = @event.Name
            });

            dbContext.SaveChanges();
        }
    }
}
