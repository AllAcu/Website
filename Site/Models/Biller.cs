using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class Biller
    {
        public static Guid AllAcuBillerId;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual IList<BillerRole> Users { get; set; } = new List<BillerRole>();
    }

    public class BillerEventHandler
        : IUpdateProjectionWhen<Domain.Biller.Biller.BillerInitialized>
    {
        private readonly AllAcuSiteDbContext dbContext;
        public BillerEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public void UpdateProjection(Domain.Biller.Biller.BillerInitialized @event)
        {
            dbContext.Billers.Add(new Biller
            {
                Id = @event.AggregateId,
                Name = @event.Name
            });

            dbContext.SaveChanges();
        }
    }
}
