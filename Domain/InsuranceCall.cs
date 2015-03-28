using System;

namespace Domain
{
    public class InsuranceCall
    {
        public User Checker { get; set; }
        public DateTimeOffset CallStart { get; set; }
        public DateTimeOffset CallComplete { get; set; }
        public string InsuranceRepresentative { get; set; }
    }
}