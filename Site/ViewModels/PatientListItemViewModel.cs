using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class PatientListItemViewModel
    {
        public Guid PatientId { get; set; }
        public Guid ProviderId { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
    }

    public class PatientListItemViewModelHandler :
        IUpdateProjectionWhen<CareProvider.NewPatient>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<CareProvider.PatientContactInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public PatientListItemViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var patient = dbContext.PatientList.First(p => p.PatientId == @event.PatientId);

            patient.Name = @event.UpdatedName ?? patient.Name;
            patient.DateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ?? patient.DateOfBirth;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientContactInformationUpdated @event)
        {
            var patient = dbContext.PatientList.First(p => p.PatientId == @event.PatientId);

            //patient.PersonalInfo.Address = @event.UpdatedAddress ?? patient.PersonalInfo.Address;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.NewPatient @event)
        {
            dbContext.PatientList.Add(new PatientListItemViewModel
            {
                PatientId = @event.PatientId,
                ProviderId = @event.AggregateId,
                Name = @event.Name,
                DateOfBirth = @event.DateOfBirth.ToShortDateString(),
            });

            dbContext.SaveChanges();
        }
    }
}
