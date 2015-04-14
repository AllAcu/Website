using System;

namespace Domain.CareProvider
{
    public class ClaimDraft
    {
        public Guid Id { get; set; }
        public Patient Patient { get; set; } = new Patient();
        public Visit Visit { get; set; } = new Visit();
    }
}
