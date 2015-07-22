using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class ConfirmEmail : Command<Registration>
        {
            public string Token { get; set; }
            public string Email { get; set; }

            public override IValidationRule<Registration> Validator
            {
                get { return Validate.That<Registration>(r => r.Confirmation.Token == Token); }
            }
        }
    }
}