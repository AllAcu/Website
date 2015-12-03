using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class SignUp : ConstructorCommand<User>
        {
            public string Email { get; set; }

            public override IValidationRule CommandValidator
            {
                get { return Validate.That<SignUp>(c => !string.IsNullOrEmpty(c.Email)); }
            }
        }
    }
}