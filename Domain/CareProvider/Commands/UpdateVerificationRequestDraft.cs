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
            public Guid DraftId { get; }
            public VerificationRequest RequestDraft { get; }

            public UpdateVerificationRequestDraft(Guid draftId, VerificationRequest requestDraft)
            {
                DraftId = draftId;
                RequestDraft = requestDraft;
            }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(p => p.PendingVerifications.Any(d => d.DraftId == DraftId && d.Status == PendingVerification.RequestStatus.Draft))
                        .WithErrorMessage((ev, pr) =>
                            pr.PendingVerifications.Any(r => r.DraftId == DraftId && r.Status == PendingVerification.RequestStatus.Submitted)
                                ? "The draft has been submitted"
                                : "This draft doesn't exist");
                }
            }
        }
    }
}
