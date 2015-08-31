using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class PatientDetails
    {
        public Guid PatientId { get; set; }
        public virtual CareProviderDetails Provider { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public LatestVerification CurrentVerification { get; set; } = new LatestVerification();

        public virtual MedicalInsuranceDetails MedicalInsurance { get; set; }
        public virtual PersonalInjuryProtectionDetails PersonalInjuryProtection { get; set; }

        public class MedicalInsuranceDetails
        {
            public long Id { get; set; }
            public string InsuranceCompany { get; set; }
            public string Plan { get; set; }
            public string ProviderPhoneNumber { get; set; }
            public string InsuranceId { get; set; }
            public string GroupNumber { get; set; }
            public bool SecondaryCoverage { get; set; }

        }
        public class PersonalInjuryProtectionDetails
        {
            public long Id { get; set; }
            public string DateOfInjury { get; set; }
            public string PlaceOfAccident { get; set; }
            public string InsuranceCarrier { get; set; }
            public string PolicyNumber { get; set; }
            public string ClaimNumber { get; set; }
            public string InsuranceCompanyAddress { get; set; }
            public string AdjusterName { get; set; }
            public string AdjusterPhone { get; set; }
            public string Injury { get; set; }
            public string Notes { get; set; }
        }

        public class LatestVerification
        {
            public Guid Id { get; set; }
            public string Status { get; set; }
        }
    }

    public class PatientDetailsViewModelHandler :
    IUpdateProjectionWhen<CareProvider.NewPatient>,
    IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
    IUpdateProjectionWhen<CareProvider.PatientContactInformationUpdated>,
    IUpdateProjectionWhen<CareProvider.InsuranceUpdated>,
    IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Started>,
    IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.RequestSubmitted>,
    IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Approved>,
    IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Rejected>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public PatientDetailsViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.NewPatient @event)
        {
            dbContext.PatientDetails.Add(new PatientDetails
            {
                PatientId = @event.PatientId,
                Provider = dbContext.CareProviders.Find(@event.AggregateId),
                Name = @event.Name,
                DateOfBirth = @event.DateOfBirth.ToShortDateString(),
                Gender = @event.Gender?.ToString(),
                Address1 = @event.Address.Line1,
                Address2 = @event.Address.Line2,
                City = @event.Address.City?.ToString(),
                State = @event.Address.State?.ToString(),
                PostalCode = @event.Address.PostalCode?.ToString(),
                PhoneNumber = @event.PhoneNumber?.ToString()
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var patient = GetPatient(@event.PatientId);

            patient.Name = @event.UpdatedName ?? patient.Name;
            patient.DateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ?? patient.DateOfBirth;
            patient.Gender = @event.UpdatedGender?.ToString() ?? patient.Gender;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientContactInformationUpdated @event)
        {
            var patient = GetPatient(@event.PatientId);

            if (@event.UpdatedAddress != null)
            {
                patient.Address1 = @event.UpdatedAddress.Line1;
                patient.Address2 = @event.UpdatedAddress.Line2;
                patient.City = @event.UpdatedAddress.City?.ToString();
                patient.State = @event.UpdatedAddress.State?.ToString();
                patient.PostalCode = @event.UpdatedAddress.PostalCode?.ToString();
            }
            patient.PhoneNumber = @event.UpdatedPhoneNumber?.ToString() ?? patient.PhoneNumber;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.InsuranceUpdated @event)
        {
            var patient = GetPatient(@event.PatientId);

            patient.MedicalInsurance = @event.MedicalInsurance == null ? null :
                new PatientDetails.MedicalInsuranceDetails
                {
                    InsuranceCompany = @event.MedicalInsurance.InsuranceCompany,
                    Plan = @event.MedicalInsurance.Plan,
                    ProviderPhoneNumber = @event.MedicalInsurance.ProviderPhoneNumber,
                    InsuranceId = @event.MedicalInsurance.InsuranceId,
                    GroupNumber = @event.MedicalInsurance.GroupNumber
                };

            patient.PersonalInjuryProtection = @event.PersonalInjuryProtection == null ? null :
                new PatientDetails.PersonalInjuryProtectionDetails
                {
                    DateOfInjury = @event.PersonalInjuryProtection.DateOfInjury.ToShortDateString(),
                    PlaceOfAccident = @event.PersonalInjuryProtection.PlaceOfAccident,
                    InsuranceCarrier = @event.PersonalInjuryProtection.InsuranceCarrier,
                    PolicyNumber = @event.PersonalInjuryProtection.PolicyNumber,
                    ClaimNumber = @event.PersonalInjuryProtection.ClaimNumber,
                    InsuranceCompanyAddress = @event.PersonalInjuryProtection.InsuranceCompanyAddress,
                    AdjusterName = @event.PersonalInjuryProtection.AdjusterName,
                    AdjusterPhone = @event.PersonalInjuryProtection.AdjusterPhone,
                    Injury = @event.PersonalInjuryProtection.Injury,
                    Notes = @event.PersonalInjuryProtection.Notes
                };

            dbContext.SaveChanges();
        }

        private PatientDetails GetPatient(Guid patientId)
        {
            return dbContext.PatientDetails.First(p => p.PatientId == patientId);
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Started @event)
        {
            var patient = GetPatient(@event.PatientId);
            patient.CurrentVerification = new PatientDetails.LatestVerification
            {
                Id = @event.AggregateId,
                Status = "Draft"
            };

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.RequestSubmitted @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            var patient = GetPatient(verification.PatientId);
            patient.CurrentVerification.Status = "Submitted";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Approved @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            var patient = GetPatient(verification.PatientId);
            patient.CurrentVerification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Rejected @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            var patient = GetPatient(verification.PatientId);
            patient.CurrentVerification.Status = "Draft";

            dbContext.SaveChanges();
        }
    }
}
