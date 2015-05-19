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
                    return Validate.That<CareProvider>(p => p.VerificationRequestDrafts.Count(d => d.DraftId == DraftId) == 1)
                        .WithErrorMessage((ev, pr) => pr.OutstandingVerifications.Any(r => r.RequestId == DraftId) ?
                            "The draft has been submitted" :
                            "This draft doesn't exist"
                            );
                }
            }
        }
    }
}
