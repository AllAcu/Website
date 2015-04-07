using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class UpdateClaim : Command<ClaimFilingProcess>
        {
            public ClaimDraft Claim { get; set; }
        }
    }
}
