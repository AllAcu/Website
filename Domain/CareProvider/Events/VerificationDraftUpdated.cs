using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class VerificationDraftUpdated : Event<CareProvider>
        {
            public Guid VerificationId { get; set; }
            public VerificationRequest Request { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.PendingVerifications.Single(r => r.Id == VerificationId)
                    .Request = Request;
            }
        }
    }
}
