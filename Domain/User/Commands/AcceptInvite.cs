using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class AcceptInvite : Command<User>
        {
            public Guid OrganizationId { get; set; }

            public override IValidationRule<User> Validator => 
                Validate.That<User>(u => u.Invitations.Any(i => i.ProviderId == OrganizationId));
        }
    }
}
