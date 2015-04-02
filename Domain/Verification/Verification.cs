using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Verification : EventSourcedAggregate<Verification>
    {
        public Verification()
        {
            
        }

        public Verification(Guid id, IEnumerable<IEvent> events) : base(id, events)
        {
            
        }
    }
}