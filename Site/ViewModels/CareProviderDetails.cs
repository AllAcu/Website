using System;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class CareProviderDetails
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string City { get; set; }
        public string NpiNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxId { get; set; }
    }

    public class CareProviderInformationHandler :
        IUpdateProjectionWhen<CareProvider.NewProvider>,
        IUpdateProjectionWhen<CareProvider.ProviderUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public CareProviderInformationHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.NewProvider @event)
        {
            dbContext.CareProviders.Add(new CareProviderDetails
            {
                Id = @event.AggregateId,
                BusinessName = @event.BusinessName,
                City = @event.City,
                NpiNumber = @event.NpiNumber,
                TaxId = @event.TaxId
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.ProviderUpdated @event)
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
