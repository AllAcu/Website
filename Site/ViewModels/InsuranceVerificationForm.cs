﻿using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class InsuranceVerificationForm
    {
        public Guid VerificationId { get; set; }
        public Guid PatientId { get; set; }
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

        public Benefits Benefits { get; set; }
    }

    public class InsuranceVerificationFormEventHandler
        : IUpdateProjectionWhen<CareProvider.VerificationRequestSubmitted>,
            IUpdateProjectionWhen<CareProvider.PatientInformationUpdated>,
            IUpdateProjectionWhen<CareProvider.VerificationUpdated>,
            IUpdateProjectionWhen<CareProvider.VerificationApproved>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationFormEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.VerificationRequestSubmitted @event)
        {
            var verification = dbContext.VerificationRequestDrafts.First(f => f.VerificationId == @event.VerificationId);
            var patient = dbContext.PatientDetails.First(p => p.PatientId == verification.PatientId);
            var provider = dbContext.CareProviders.First(p => p.Id == @event.AggregateId);

            var form = new InsuranceVerificationForm
            {
                VerificationId = @event.VerificationId,
                PatientId = patient.PatientId,
                InsuranceCarrier = patient.MedicalInsurance != null ? patient.MedicalInsurance.InsuranceCompany : patient.PersonalInjuryProtection.InsuranceCarrier,
                InsurancePhoneNumber = patient.MedicalInsurance != null ? patient.MedicalInsurance.ProviderPhoneNumber : patient.PersonalInjuryProtection.AdjusterPhone,
                Acupuncturist = provider.PractitionerName,
                AcupuncturistNpiNumber = provider.NpiNumber,
                AcupuncturistAddress = provider.Address,
                AcupuncturistPhoneNumber = provider.PhoneNumber,
                AcupuncturistTaxId = provider.TaxId,
                PatientName = patient.Name,
                PatientDateOfBirth = patient.DateOfBirth,
                PatientInsurancePolicy = patient.MedicalInsurance != null ? patient.MedicalInsurance.Plan : patient.PersonalInjuryProtection.ClaimNumber,
                Benefits = new Benefits()
            };

            dbContext.VerificationForms.Add(form);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.PatientInformationUpdated @event)
        {
            var form = dbContext.VerificationForms.SingleOrDefault(f => f.PatientId == @event.PatientId);

            if (form != null)
            {
                form.PatientName = @event.UpdatedName ?? form.PatientName;
                form.PatientDateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ?? form.PatientDateOfBirth;
            }
        }

        public void UpdateProjection(CareProvider.VerificationUpdated @event)
        {
            var form = dbContext.VerificationForms.Find(@event.VerificationId);
            form.Benefits = @event.Benefits.ToBenefits();

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.VerificationApproved @event)
        {
            var form = dbContext.VerificationForms.Find(@event.VerificationId);
            dbContext.VerificationForms.Remove(form);

            dbContext.SaveChanges();
        }
    }

}
