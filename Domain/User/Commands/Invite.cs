using System;
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
            public Guid ProviderId { get; set; }
            public Role Role { get; set; }

            public override IValidationRule CommandValidator
            {
                get
                {
                    return Validate.That<Invite>(i =>
                        i.ProviderId != Guid.Empty &&
                        !string.IsNullOrEmpty(i.Email) &&
                        i.Role != null);
                }
            }
        }
    }
}
