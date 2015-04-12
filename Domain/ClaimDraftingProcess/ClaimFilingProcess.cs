using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess : EventSourcedAggregate<ClaimFilingProcess>
    {
        public static Func<DateTimeOffset> Now = () => DateTimeOffset.UtcNow;

        public ClaimFilingProcess()
        {
            
        }

        public ClaimFilingProcess(Guid id, IEnumerable<IEvent> events) : base(id, events)
        {
            
        }

        public ClaimDraft Claim { get; private set; }
        public bool HasBeenSubmitted { get; private set; } = false;
    }
}