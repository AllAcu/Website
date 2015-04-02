using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class StartClaim : Command<ClaimFilingProcess>
        {
            public ClaimDraft Claim { get; set; }
        }
    }
}
