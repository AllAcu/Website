using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User : EventSourcedAggregate<User>
    {
        public Func<DateTime> Now = () => DateTime.UtcNow;

        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public bool Confirmed { get; protected set; }

        public IList<Invitation> Invitations { get; protected set; } = new List<Invitation>();

        public User(Guid? id = default(Guid?)) : base(id)
        {
        }

        public User(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public User(ConstructorCommand<User> createCommand) : base(createCommand)
        {
        }
    }
}
