using System;

namespace Domain
{
    public class ClaimDraft
    {
        public Guid Id { get; set; }
        public string Patient { get; set; }
        public CareProvider Provider { get; set; }
        public Visit Visit { get; set; } = new Visit();
    }
}
