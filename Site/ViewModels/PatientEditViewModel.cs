using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Models.Providers
{
    public class PatientEditViewModel
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class PatientEditViewModelHandler :
        IUpdateProjectionWhen<CareProvider.NewPatient>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public PatientEditViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.NewPatient @event)
        {
            dbContext.PatientEditViewModels.Add(new PatientEditViewModel
            {
                PatientId = @event.PatientId,
                Name = @event.Name,
                DateOfBirth = @event.DateOfBirth,
                Gender = @event.Gender
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var patient = dbContext.PatientEditViewModels.First(p => p.PatientId == @event.PatientId);

            patient.Name = @event.UpdatedName ?? patient.Name;
            patient.DateOfBirth = @event.UpdatedDateOfBirth ?? patient.DateOfBirth;
            patient.Gender = @event.UpdatedGender?.ToString() ?? patient.Gender;

            dbContext.SaveChanges();
        }
    }
}
