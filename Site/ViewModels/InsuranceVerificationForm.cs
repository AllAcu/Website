using System;
using System.Linq;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class InsuranceVerificationForm
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
        IUpdateProjectionWhen<InsuranceVerification.VerificationStarted>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationDraftUpdated>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationRequestSubmitted>,
        IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationUpdated>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationApproved>,
        IUpdateProjectionWhen<InsuranceVerification.VerificationRevised>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationFormEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(InsuranceVerification.VerificationStarted @event)
        {
            var request = new InsuranceVerificationForm
            {
                PatientId = @event.PatientId,
                VerificationId = @event.AggregateId,
                Request = @event.Request ?? new VerificationRequest(),
                Status = "Draft"
            };

            dbContext.VerificationForms.Add(request);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationDraftUpdated @event)
        {
            var verification = dbContext.VerificationForms.First(f => f.VerificationId == @event.AggregateId);
            verification.Request = @event.Request;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationRequestSubmitted @event)
        {
            var verification = dbContext.VerificationForms.First(f => f.VerificationId == @event.AggregateId);
            var patient = dbContext.PatientDetails.First(p => p.PatientId == verification.PatientId);
            var provider = dbContext.CareProviders.First(p => p.Id == patient.ProviderId);

            verification.Status = "Submitted";
            verification.Patient = new InsuranceVerificationForm.PatientInfo
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
            var form = dbContext.VerificationForms.SingleOrDefault(f => f.PatientId == @event.PatientId);

            if (form != null)
            {
                form.Patient.PatientName = @event.UpdatedName ?? form.Patient.PatientName;
                form.Patient.PatientDateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ??
                                                  form.Patient.PatientDateOfBirth;
            }
        }

        public void UpdateProjection(InsuranceVerification.VerificationUpdated @event)
        {
            var form = dbContext.VerificationForms.Find(@event.AggregateId);
            form.Benefits = @event.Benefits.ToBenefits();

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationApproved @event)
        {
            var form = dbContext.VerificationForms.Find(@event.AggregateId);
            dbContext.VerificationForms.Remove(form);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(InsuranceVerification.VerificationRevised @event)
        {
            var form = dbContext.VerificationForms.First(f => f.VerificationId == @event.AggregateId);
            form.Status = "Draft";

            dbContext.SaveChanges();
        }
    }
}
