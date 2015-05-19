using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class VerificationDraftUpdated : Event<CareProvider>
        {
            public Guid DraftId { get; set; }
            public VerificationRequest Request { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.VerificationRequestDrafts.Single(r => r.DraftId == DraftId)
                    .Request = Request;
            }
        }
    }
}
