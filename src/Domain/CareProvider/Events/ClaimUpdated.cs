using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class ClaimUpdated : Event<CareProvider>
        {
            public ClaimDraft Claim { get; }

            public ClaimUpdated(ClaimDraft claim)
            {
                Claim = claim;
            }

            public override void Update(CareProvider provider)
            {
            }
        }
    }
}