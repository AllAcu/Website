using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdateVerificationRequestDraft : Command<CareProvider>
        {
            public Guid VerificationId { get; }
            public VerificationRequest RequestDraft { get; }

            public UpdateVerificationRequestDraft(Guid verificationId, VerificationRequest requestDraft)
            {
                VerificationId = verificationId;
                RequestDraft = requestDraft;
            }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(p => p.PendingVerifications.Any(d => d.Id == VerificationId && d.Status == PendingVerification.RequestStatus.Draft))
                        .WithErrorMessage((ev, pr) =>
                            pr.PendingVerifications.Any(r => r.Id == VerificationId && r.Status == PendingVerification.RequestStatus.Submitted)
                                ? "The draft has been submitted"
                                : "This draft doesn't exist");
                }
            }
        }
    }
}
