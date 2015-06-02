using System;

namespace AllAcu.Models.Providers
{
    public class PatientDetails
    {
        public Guid PatientId { get; set; }
        public Guid ProviderId { get; set; }
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
}
