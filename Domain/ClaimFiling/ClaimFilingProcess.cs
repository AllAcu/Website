using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.ClaimFiling
{
    public partial class ClaimFilingProcess : EventSourcedAggregate<ClaimFilingProcess>
    {
        public static Func<DateTimeOffset> Now = () => DateTimeOffset.UtcNow;

        public ClaimFilingProcess(Guid id, IEnumerable<IEvent> events) : base(id, events)
        {
            
        }

        public Claim Claim { get; }
        public bool HasBeenSubmitted { get; private set; }
    }
}