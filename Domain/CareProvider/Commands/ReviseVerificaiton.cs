using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class ReviseVerification : Command<CareProvider>
        {
            public Guid VerificationId { get; set; }
            public string Reason { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(p => p.PendingVerifications.Any(d => d.Id == VerificationId && d.Status == PendingVerification.RequestStatus.Submitted))
                        .WithErrorMessage((ev, pr) =>
                            pr.PendingVerifications.Any(r => r.Id == VerificationId && r.Status == PendingVerification.RequestStatus.Draft)
                                ? "The draft has not been submitted"
                                : "This draft doesn't exist");
                }
            }
        }
    }
}
