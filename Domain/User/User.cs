using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User : EventSourcedAggregate<User>
    {
        public string Name { get; set; }

        public User(Guid? id = default(Guid?)) : base(id)
        {
        }

        public User(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public User(Create command) : base(command.AggregateId)
        {
            RecordEvent(new Registered());
        }
    }
}
