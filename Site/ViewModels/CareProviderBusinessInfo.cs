using System;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class CareProviderBusinessInfo
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string PractitionerName { get; set; }
        public string City { get; set; }
        public string NpiNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxId { get; set; }
    }

    public class CareProviderInformationHandler : IUpdateProjectionWhen<CareProvider.NewProvider>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public CareProviderInformationHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.NewProvider @event)
        {
            dbContext.CareProviders.Add(new CareProviderBusinessInfo
            {
                Id = @event.AggregateId,
                BusinessName = @event.BusinessName,
                PractitionerName = @event.PractitionerName,
                City = @event.City,
                NpiNumber = @event.NpiNumber,
                TaxId = @event.TaxId
            });

            dbContext.SaveChanges();
        }
    }

}
