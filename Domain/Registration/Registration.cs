using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration : EventSourcedAggregate<Registration>
    {
        public string Email { get; set; }
        public IList<Invitation> Invitations { get; set; } = new List<Invitation>();
        public EmailConfirmation Confirmation { get; set; }
        public RegistrationInfo Info { get; set; }
        public Guid? UserId { get; set; }

        public Registration(SignUp command) : base(command)
        {
            Email = command.Email;
            Confirmation = new EmailConfirmation
            {
                ConfirmationSentDate = Now(),
                Token = GenerateToken()
            };
        }

        public Registration(Invite command) : base(command)
        {
            RecordEvent(new FirstUserInvite
            {
                Email = command.Email,
                ConfirmationSentDate = Now(),
                Token = GenerateToken()
            });
        }

        public Registration(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        private string GenerateToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");
        }
    }
}
