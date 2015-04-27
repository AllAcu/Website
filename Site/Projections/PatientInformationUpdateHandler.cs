using System;
using System.Linq;
using AllAcu.Models.Providers;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Projections
{
    public class PatientInformationUpdateHandler :
        IUpdateProjectionWhen<CareProvider.NewPatient>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<CareProvider.PatientContactInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public PatientInformationUpdateHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var patient = dbContext.Patients.First(p => p.PatientId == @event.PatientId);

            patient.PersonalInfo.Name = @event.UpdatedName ?? patient.PersonalInfo.Name;
            patient.PersonalInfo.DateOfBirth = @event.UpdatedDateOfBirth ?? patient.PersonalInfo.DateOfBirth;
            patient.PersonalInfo.Gender = @event.UpdatedGender?.ToString() ?? patient.PersonalInfo.Gender;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientContactInformationUpdated @event)
        {
            var patient = dbContext.Patients.First(p => p.PatientId == @event.PatientId);

            patient.PersonalInfo.Address = @event.UpdatedAddress ?? patient.PersonalInfo.Address;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.NewPatient @event)
        {
            dbContext.Patients.Add(new Patient
            {
                PatientId = @event.PatientId,
                ProviderId = @event.AggregateId,
                PersonalInfo = new PatientPersonalInformation
                {
                    Name = @event.Name,
                    DateOfBirth = DateTime.Parse("1/1/1970")
                }
            });

            dbContext.SaveChanges();
        }
    }
}
