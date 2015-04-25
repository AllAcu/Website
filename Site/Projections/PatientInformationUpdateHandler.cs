using System.Linq;
using AllAcu.Models.Patients;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Projections
{
    public class PatientInformationUpdateHandler :
        IUpdateProjectionWhen<CareProvider.NewPatient>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<CareProvider.PatientContactInformationUpdated>
    {
        private readonly PatientDbContext dbContext;

        public PatientInformationUpdateHandler(PatientDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var patient = dbContext.Patients.First(p => p.Id == @event.PatientId);

            patient.Name = @event.UpdatedName ?? patient.Name;
            patient.DateOfBirth = @event.UpdatedDateOfBirth?.ToString("d") ?? patient.DateOfBirth;
            patient.Gender = @event.UpdatedGender?.ToString() ?? patient.Gender;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientContactInformationUpdated @event)
        {
            var patient = dbContext.Patients.First(p => p.Id == @event.PatientId);

            patient.Address = @event.UpdatedAddress ?? patient.Address;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.NewPatient @event)
        {
            dbContext.Patients.Add(new PatientPersonalInformation
            {
                Id = @event.PatientId,
                Name = @event.Name
            });

            dbContext.SaveChanges();
        }
    }
}
