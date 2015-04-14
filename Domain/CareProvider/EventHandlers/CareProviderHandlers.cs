using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace Domain.CareProvider.EventHandlers
{
    public class CareProviderHandlers :
        IUpdateProjectionWhen<CareProvider.NewProvider>,
        IUpdateProjectionWhen<CareProvider.NewPatient>
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

        public void UpdateProjection(CareProvider.NewPatient @event)
        {
            dbContext.Patients.Add(new Patient(@event.PatientId) { Name = @event.Name });
            dbContext.SaveChanges();
        }
    }
}
