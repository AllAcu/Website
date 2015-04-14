using System;

namespace Domain
{
    public class InsurancePolicy
    {
        public string InsuranceCompany { get; set; }
        public string Plan { get; set; }
        public DateTimeOffset IssueDate { get; set; }
        public DateTimeOffset PolicyDate { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public DateTimeOffset TerminationDate { get; set; }
    }
}