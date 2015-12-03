using System;

namespace Domain.ClaimFiling
{
    public class InsuranceCall
    {
        public Guid Checker { get; set; }
        public DateTimeOffset CallStart { get; set; }
        public DateTimeOffset CallComplete { get; set; }
        public string InsuranceRepresentative { get; set; }
    }
}