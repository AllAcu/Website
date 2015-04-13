using Domain.CareProvider;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdateClaim : Command<CareProvider>
        {
            public ClaimDraft Claim { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(p => true);
                }
            }
        }
    }
}
