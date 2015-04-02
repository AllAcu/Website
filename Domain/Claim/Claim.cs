using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class Claim : EventSourcedAggregate<Claim>
    {
        public Claim()
        {
            
        }

        public Claim(Guid id, IEnumerable<IEvent> events) : base(id, events)
        {
            
        }
        ClaimId ClaimNumber { get; set; }

    }
}