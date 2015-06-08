using System;
using System.Linq;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class InsuranceVerification
    {
        public Guid VerificationId { get; set; }
        public Guid PatientId { get; set; }
        public string Status { get; set; }

        public VerificationRequest Request { get; set; }
        public PatientInfo Patient { get; set; } = new PatientInfo();
        public Benefits Benefits { get; set; } = new Benefits();

        public class PatientInfo
        {
            public string InsuranceCarrier { get; set; }
            public string InsurancePhoneNumber { get; set; }
            public string Acupuncturist { get; set; }
            public string AcupuncturistNpiNumber { get; set; }
            public string AcupuncturistTaxId { get; set; }
            public string AcupuncturistAddress { get; set; }
            public string AcupuncturistPhoneNumber { get; set; }
            public string PatientName { get; set; }
            public string PatientDateOfBirth { get; set; }
            public string PatientInsurancePolicy { get; set; }
        }
    }

    public class InsuranceVerificationFormEventHandler :
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationStarted>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationDraftUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationRequestSubmitted>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationApproved>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.VerificationRevised>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationFormEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationStarted @event)
        {
            var request = new InsuranceVerification
            {
                PatientId = @event.PatientId,
                VerificationId = @event.AggregateId,
                Request = @event.Request ?? new VerificationRequest(),
                Status = "Draft"
            };

            dbContext.Verifications.Add(request);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationDraftUpdated @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            verification.Request = @event.Request;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationRequestSubmitted @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            var patient = dbContext.PatientDetails.First(p => p.PatientId == verification.PatientId);
            var provider = dbContext.CareProviders.First(p => p.Id == patient.ProviderId);

            verification.Status = "Submitted";
            verification.Patient = new InsuranceVerification.PatientInfo
            {
                InsuranceCarrier = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.InsuranceCompany
                    : patient.PersonalInjuryProtection.InsuranceCarrier,
                InsurancePhoneNumber = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.ProviderPhoneNumber
                    : patient.PersonalInjuryProtection.AdjusterPhone,
                Acupuncturist = provider.PractitionerName,
                AcupuncturistNpiNumber = provider.NpiNumber,
                AcupuncturistAddress = provider.Address,
                AcupuncturistPhoneNumber = provider.PhoneNumber,
                AcupuncturistTaxId = provider.TaxId,
                PatientName = patient.Name,
                PatientDateOfBirth = patient.DateOfBirth,
                PatientInsurancePolicy = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.Plan
                    : patient.PersonalInjuryProtection.ClaimNumber
            };

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var verification = dbContext.Verifications.SingleOrDefault(f => f.PatientId == @event.PatientId);

            if (verification != null)
            {
                verification.Patient.PatientName = @event.UpdatedName ?? verification.Patient.PatientName;
                verification.Patient.PatientDateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ??
                                                  verification.Patient.PatientDateOfBirth;
            }
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationUpdated @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Benefits = @event.Benefits.ToBenefits();

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationApproved @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.VerificationRevised @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            verification.Status = "Draft";

            dbContext.SaveChanges();
        }
    }
}
