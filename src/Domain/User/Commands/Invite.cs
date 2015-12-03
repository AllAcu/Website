using System;
using System.Linq;
using Domain.Authentication;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Invite : Command<User>
        {
            public string Email { get; set; }
            public Guid OrganizationId { get; set; }
            public Role Role { get; set; }

            public override IValidationRule CommandValidator
            {
                get
                {
                    return Validate.That<Invite>(i =>
                        i.OrganizationId != Guid.Empty &&
                        i.Role != null);
                }
            }

            public override IValidationRule<User> Validator
            {
                get
                {
                    return Validate.That<User>(u => !u.HasBeenInvited(OrganizationId, Role));
                }
            }
        }
    }

    public static class User_Extensions
    {
        public static bool HasBeenInvited(this User user, Guid providerId, Role role)
        {
            return user.Invitations.FirstOrDefault(i => i.OrganizationId == providerId)?.Roles.Contains(role) == true;
        }
    }
}
