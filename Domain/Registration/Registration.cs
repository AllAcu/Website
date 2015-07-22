using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration : EventSourcedAggregate<Registration>
    {
        public Func<DateTime> Now = () => DateTime.UtcNow;
        public string Email { get; set; }
        public IList<Invitation> Invitations { get; set; } = new List<Invitation>();
        public EmailConfirmation Confirmation { get; set; }
        public string Name { get; set; }
        public Guid? UserId { get; set; }

        public Registration(Invite command) : base(command.AggregateId != Guid.Empty ? command.AggregateId : Guid.NewGuid())
        {
            RecordEvent(new FirstUserInvite
            {
                Email = command.Email,
                ConfirmationSentDate = Now(),
                Token = GenerateToken()
            });
        }

        public Registration(ConstructorCommand<Registration> createCommand) : base(createCommand)
        {
        }

        public Registration(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        protected static string GenerateToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");
        }
    }
}
