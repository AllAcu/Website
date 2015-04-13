using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace Domain.CareProvider.EventHandlers
{
    public class CareProviderHandlers :IUpdateProjectionWhen<CareProvider.NewProvider>
    {
        private readonly CareProviderReadModelDbContext dbContext;

        public CareProviderHandlers(CareProviderReadModelDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.NewProvider @event)
        {
            dbContext.CareProviders.Add(new CareProviderInfo
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
