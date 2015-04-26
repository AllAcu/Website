using Domain.CareProvider;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace AllAcu.Projections
{
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
                City = @event.City
            });

            dbContext.SaveChanges();
        }
    }
}
