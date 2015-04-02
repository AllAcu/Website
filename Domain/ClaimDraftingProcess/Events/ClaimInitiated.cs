using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class ClaimInitiated : Event<ClaimFilingProcess>
        {
            public ClaimDraft Claim { get; }

            public ClaimInitiated(ClaimDraft claim)
            {
                this.Claim = claim;
            }

            public override void Update(ClaimFilingProcess process)
            {
                process.Claim = Claim;
            }
        }
    }
}
