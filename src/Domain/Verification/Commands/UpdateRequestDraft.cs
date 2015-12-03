using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class UpdateRequestDraft : Command<InsuranceVerification>
        {
            public VerificationRequest RequestDraft { get; set; }
        }
    }
}
