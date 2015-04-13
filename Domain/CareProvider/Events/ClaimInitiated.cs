using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class ClaimStarted : Event<CareProvider>
        {
            public ClaimDraft Claim { get; }

            public ClaimStarted(ClaimDraft claim)
            {
                Claim = claim;
            }

            public override void Update(CareProvider provider)
            {
                provider.Drafts.Add(Claim);
            }
        }
    }
}
