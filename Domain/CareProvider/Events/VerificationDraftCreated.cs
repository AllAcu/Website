using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class VerificationDraftCreated : Event<CareProvider>
        {
            public Guid DraftId { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.PendingVerifications.Add(new PendingVerification
                {
                    DraftId = DraftId
                });
            }
        }
    }
}
