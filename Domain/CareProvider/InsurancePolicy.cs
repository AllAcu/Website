using System;
using Domain.CareProvider;

namespace Domain
{
    public class InsurancePolicy<TInsurance> : InsurancePolicy
    {
        public TInsurance Insurance { get; }

        public InsurancePolicy(TInsurance insurance)
        {
            Insurance = insurance;
        }
    }

    public class MedicalInsurance
    {
        public string InsuranceCompany { get; set; }
        public string Plan { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public string InsuranceId { get; set; }
        public string GroupNumber { get; set; }
        public bool SecondaryCoverage { get; set; }
    }

    public class PersonalInjuryProtection
    {
        public DateTime DateOfInjury { get; set; }
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

    public class Verification
    {
        public string Verifier { get; set; }
        public DateTimeOffset VerifiedOn { get; set; }
    }

    public abstract class InsurancePolicy
    {
        // relationship to policy for a patient?


        public Verification Verification { get; set; }
        public DateTimeOffset IssueDate { get; set; }
        public DateTimeOffset PolicyDate { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public DateTimeOffset? TerminationDate { get; set; }
    }
}
