using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class VerificationRevised : Event<CareProvider>
        {
            public Guid VerificationId { get; set; }
            public string Reason { get; set; }

            public override void Update(CareProvider provider)
            {
                var verification = provider.PendingVerifications.Single(p => p.Id == VerificationId);
                verification.Status = PendingVerification.RequestStatus.Draft;
            }
        }
    }
}
