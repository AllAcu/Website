using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class SubmitVerificationRequest : Command<CareProvider>
        {
            public Guid? DraftId { get; set; }
            public VerificationRequest VerificationRequest { get; set; }

            public override IValidationRule CommandValidator
            {
                get
                {
                    return Validate.That<SubmitVerificationRequest>(
                        command => command.DraftId != null || VerificationRequest != null)
                        .WithMessage("Must supply a request if not submitting an existing one");
                }
            }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(p => DraftId == null || p.VerificationRequestDrafts.Any(d => d.DraftId == DraftId))
                        .WithErrorMessage((ev, pr) => pr.OutstandingVerifications.Any(r => r.RequestId == DraftId) ?
                            "The draft has been submitted" :
                            "This draft doesn't exist"
                            );
                }
            }
        }
    }
}
