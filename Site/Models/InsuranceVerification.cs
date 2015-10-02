﻿using System;
using System.Linq;
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

        public string Approval { get; set; }
        public string ServiceCenterRepresentative { get; set; }
        public string CallReferenceNumber { get; set; }
        public string CallTime { get; set; }
        public virtual CareProvider Provider { get; set; }
        public virtual User AssignedTo { get; set; }
        public DateTimeOffset? ApprovedTimestamp { get; set; }
    }

    public class InsuranceVerificationEventHandler :
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.CallStarted>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.DraftUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.RequestSubmitted>,
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.PatientInformationUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Updated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Completed>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.RequestRejected>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Delegated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.CallStarted @event)
        {
            var request = new InsuranceVerification
            {
                PatientId = @event.PatientId,
                VerificationId = @event.AggregateId,
                Provider = dbContext.Patients.Find(@event.PatientId).Provider,
                Request = @event.Request ?? new VerificationRequest(),
                Status = "Draft"
            };

            dbContext.Verifications.Add(request);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.DraftUpdated @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            verification.Request = @event.Request;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.RequestSubmitted @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            var patient = dbContext.Patients.First(p => p.PatientId == verification.PatientId);
            var provider = patient.Provider;

            verification.Status = "Submitted";
            verification.Patient = new InsuranceVerification.PatientInfo
            {
                InsuranceCarrier = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.InsuranceCompany
                    : patient.PersonalInjuryProtection.InsuranceCarrier,
                InsurancePhoneNumber = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.ProviderPhoneNumber
                    : patient.PersonalInjuryProtection.AdjusterPhone,
                Acupuncturist = "Jimmy Needles",
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

        public void UpdateProjection(Domain.CareProvider.CareProvider.PatientInformationUpdated @event)
        {
            var verification = dbContext.Verifications.SingleOrDefault(f => f.PatientId == @event.PatientId);

            if (verification != null)
            {
                verification.Patient.PatientName = @event.UpdatedName ?? verification.Patient.PatientName;
                verification.Patient.PatientDateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ??
                                                  verification.Patient.PatientDateOfBirth;
            }
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Updated @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Benefits = @event.Benefits.ToBenefits();

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Completed @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Status = "Approved";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.RequestRejected @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            verification.Status = "Draft";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Delegated @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.AssignedTo = dbContext.Users.Find(@event.UserId);

            dbContext.SaveChanges();
        }
    }
}
