using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class ClaimStarted : Event<CareProvider>
        {
            public ClaimDraft Draft { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.Drafts.Add(Draft);
            }
        }
    }
}
