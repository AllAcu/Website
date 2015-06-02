using System;
using System.Linq;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu.Models.Providers
{
    public class PatientDetailsViewModelHandler :
        IUpdateProjectionWhen<CareProvider.NewPatient>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<CareProvider.PatientContactInformationUpdated>,
        IUpdateProjectionWhen<CareProvider.InsuranceUpdated>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationStarted>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationRequestSubmitted>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationApproved>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationRevised>
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
                ProviderId = @event.AggregateId,
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

        public void UpdateProjection(InsuranceVerification.VerificationStarted @event)
        {
            var patient = GetPatient(@event.PatientId);
            patient.CurrentVerification = new PatientDetails.LatestVerification
            {
                Id = @event.AggregateId,
                Status = "Draft"
            };

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationRequestSubmitted @event)
        {
            var verification = dbContext.VerificationList.First(v => v.VerificationId == @event.AggregateId);
            var patient = GetPatient(verification.PatientId);
            patient.CurrentVerification.Status = "Submitted";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationApproved @event)
        {
            var verification = dbContext.VerificationList.First(v => v.VerificationId == @event.AggregateId);
            var patient = GetPatient(verification.PatientId);
            patient.CurrentVerification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationRevised @event)
        {
            var verification = dbContext.VerificationList.First(v => v.VerificationId == @event.AggregateId);
            var patient = GetPatient(verification.PatientId);
            patient.CurrentVerification.Status = "Draft";

            dbContext.SaveChanges();
        }
    }
}
