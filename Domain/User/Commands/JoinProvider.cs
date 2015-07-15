using System;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class JoinProvider : Command<User>
        {
            public Guid ProviderId { get; set; }

            public override IValidationRule<User> Validator
            {
                get { return Validate.That<User>(u => !u.HasPermission(ProviderId)); }
            }
        }
    }
}
