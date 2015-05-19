using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class VerificationRequestSubmitted : Event<CareProvider>
        {
            public Guid DraftId { get; set; }
            public override void Update(CareProvider provider)
            {
                var draft = provider.VerificationRequestDrafts.Single(d => d.DraftId == DraftId);
                provider.VerificationRequestDrafts.Remove(draft);

                provider.OutstandingVerifications.Add(new OutstandingVerification
                {
                    RequestId = DraftId,
                    Request = draft.Request
                });
            }
        }
    }
}
