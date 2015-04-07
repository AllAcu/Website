using System;
using System.Collections.Generic;

namespace Domain
{
    public class ClaimDraft
    {
        public Guid Id { get; set; }
        public string Patient { get; set; }
        public CareProvider Provider { get; set; }
        public string Diagnosis { get; set; }
        public DateTimeOffset DateOfService { get; set; }
        public IList<Procedure> Procedures { get; set; }
    }
}
