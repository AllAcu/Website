using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User : EventSourcedAggregate<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public User(Guid? id = default(Guid?)) : base(id)
        {
        }

        public User(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public User(Register command) : base(command.AggregateId != Guid.Empty ? command.AggregateId : Guid.NewGuid())
        {
            RecordEvent(new Registered
            {
                Name = command.Name,
                Email = command.Email
            });
        }
    }
}
