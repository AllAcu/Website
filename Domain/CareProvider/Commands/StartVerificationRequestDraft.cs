using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class StartVerificationRequestDraft : Command<CareProvider>
        {
            public StartVerificationRequestDraft(VerificationRequest requestDraft)
            {
                RequestDraft = requestDraft;
            }

            public VerificationRequest RequestDraft { get; }

            public override IValidationRule CommandValidator
            {
                get
                {
                    return Validate.That<StartVerificationRequestDraft>(command => command.RequestDraft != null)
                        .WithMessage("Must supply a draft validation");
                }
            }
        }
    }
}
