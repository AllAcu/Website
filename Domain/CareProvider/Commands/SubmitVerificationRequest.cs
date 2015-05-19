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
            public SubmitVerificationRequest(Guid? draftId = null, VerificationRequest verificationRequest = null)
            {
                VerificationRequest = verificationRequest;
                DraftId = draftId;
            }

            public Guid? DraftId { get; }
            public VerificationRequest VerificationRequest { get; }

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
                    return Validate.That<CareProvider>(p => DraftId == null || p.PendingVerifications.Any(d => d.DraftId == DraftId && d.Status == PendingVerification.RequestStatus.Draft))
                        .WithErrorMessage((ev, pr) => 
                            pr.PendingVerifications.Any(r => r.DraftId == DraftId && r.Status == PendingVerification.RequestStatus.Submitted)
                                ? "The draft has been submitted"
                                : "This draft doesn't exist");
                }
            }
        }
    }
}
