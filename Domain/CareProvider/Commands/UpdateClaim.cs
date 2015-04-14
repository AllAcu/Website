using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdateClaimDraft : Command<CareProvider>
        {
            public ClaimDraft Claim { get; set; }
        }
    }
}
