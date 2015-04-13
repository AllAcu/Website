using System;

namespace Domain.CareProvider
{
    public class ClaimDraft
    {
        public Guid Id { get; set; }
        public Patient Patient { get; set; } = new Patient();
        public CareProvider Provider { get; set; } = new CareProvider();
        public Visit Visit { get; set; } = new Visit();
    }
}
