using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Models.Providers
{
    public class PatientDetailsViewModelHandler :
        IUpdateProjectionWhen<CareProvider.NewPatient>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<CareProvider.PatientContactInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public PatientDetailsViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.NewPatient @event)
        {
            dbContext.PatientDetailsViewModels.Add(new PatientDetailsViewModel
            {
                PatientId = @event.PatientId,
                Name = @event.Name,
                DateOfBirth = @event.DateOfBirth.ToShortDateString(),
                Gender = @event.Gender,
                Address = @event.Address.Line1 + " " + @event.Address.Line2,
                City = @event.Address.City?.ToString(),
                State = @event.Address.State?.ToString(),
                PostalCode = @event.Address.PostalCode?.ToString(),
                PhoneNumber = @event.PhoneNumber?.ToString()
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var patient = dbContext.PatientDetailsViewModels.First(p => p.PatientId == @event.PatientId);

            patient.Name = @event.UpdatedName ?? patient.Name;
            patient.DateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ?? patient.DateOfBirth;
            patient.Gender = @event.UpdatedGender?.ToString() ?? patient.Gender;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientContactInformationUpdated @event)
        {
            var patient = dbContext.PatientDetailsViewModels.First(p => p.PatientId == @event.PatientId);

            //patient.PersonalInfo.Address = @event.UpdatedAddress ?? patient.PersonalInfo.Address;

            dbContext.SaveChanges();
        }
    }

    public class PatientDetailsViewModel
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
