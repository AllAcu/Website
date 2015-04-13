using System;
using System.Collections.Generic;

namespace Domain.ClaimFiling
{
    public class ClaimSubmissionRequest
    {
        public Guid Id { get; set; }
        //public Patient Patient { get; set; }
        public string Diagnosis { get; set; }
        public DateTimeOffset DateOfService { get; set; }
    }
}
