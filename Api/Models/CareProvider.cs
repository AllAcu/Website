using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class CareProvider
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string City { get; set; }
        public string NpiNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxId { get; set; }

        public virtual IList<ProviderRole> Users { get; set; } = new List<ProviderRole>();
    }

    public class CareProviderEventHandler :
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.NewProvider>,
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.ProviderUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public CareProviderEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.CareProvider.CareProvider.NewProvider @event)
        {
            dbContext.CareProviders.Add(new CareProvider
            {
                Id = @event.AggregateId,
                BusinessName = @event.BusinessName,
                City = @event.City,
                NpiNumber = @event.NpiNumber,
                TaxId = @event.TaxId
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.CareProvider.CareProvider.ProviderUpdated @event)
        {
            var provider = dbContext.CareProviders.Find(@event.AggregateId);

            provider.BusinessName = @event.BusinessName;
            provider.City = @event.City;
            provider.NpiNumber = @event.NpiNumber;
            provider.TaxId = @event.TaxId;

            dbContext.SaveChanges();
        }
    }
}
