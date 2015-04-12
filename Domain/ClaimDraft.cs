using System;

namespace Domain
{
    public class ClaimDraft
    {
        public Guid Id { get; set; }
        public Patient Patient { get; set; } = new Patient();
        public CareProvider Provider { get; set; }
        public Visit Visit { get; set; } = new Visit();
    }
}
