using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class UpdateClaim : Command<ClaimFilingProcess>
        {
            public ClaimDraft Claim { get; set; }

            public override IValidationRule<ClaimFilingProcess> Validator
            {
                get
                {
                    return Validate.That<ClaimFilingProcess>(p => !p.HasBeenSubmitted);
                }
            }
        }
    }
}
