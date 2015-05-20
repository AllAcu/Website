using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class VerificationRequestSubmitted : Event<CareProvider>
        {
            public Guid VerificationId { get; set; }
            public override void Update(CareProvider provider)
            {
                var draft = provider.PendingVerifications.Single(d => d.Id == VerificationId);
                draft.Status = PendingVerification.RequestStatus.Submitted;
            }
        }
    }
}
