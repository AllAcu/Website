using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class StartClaim : Command<CareProvider>
        {
            public ClaimDraft Claim { get; set; }
        }
    }
}
