using System;
using System.Collections.Generic;

namespace Domain
{
    public class SubmittedClaim
    {
        public Guid Id { get; set; }
        public string Patient { get; set; }
        public string Diagnosis { get; set; }
        public DateTimeOffset DateOfService { get; set; }
    }
}
