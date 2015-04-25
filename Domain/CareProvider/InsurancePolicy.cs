using System;
using Domain.CareProvider;

namespace Domain
{
    public class InsurancePolicy
    {
        // relationship to policy for a patient?
        // medical or pip

        public Verification Verification { get; set; }

        public string InsuranceCompany { get; set; }
        public string Plan { get; set; }
        public PhoneNumber InsuranceProviderNumber { get; set; }
        public InsuranceId InsuranceId { get; set; }
        public GroupNumber GroupNumber { get; set; }
        public bool SecondaryCoverage { get; set; }

        public DateTimeOffset IssueDate { get; set; }
        public DateTimeOffset PolicyDate { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public DateTimeOffset TerminationDate { get; set; }
    }

    public class Verification
    {
        public string Verifier { get; set; }
        public DateTimeOffset VerifiedOn { get; set; }
    }
}