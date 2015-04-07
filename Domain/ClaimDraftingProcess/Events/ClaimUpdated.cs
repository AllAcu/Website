using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class ClaimUpdated : Event<ClaimFilingProcess>
        {
            public ClaimDraft Claim { get; }

            public ClaimUpdated(ClaimDraft claim)
            {
                Claim = claim;
            }

            public override void Update(ClaimFilingProcess process)
            {
                process.Claim = Claim;
            }
        }
    }
}