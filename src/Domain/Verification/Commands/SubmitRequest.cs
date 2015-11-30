using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class SubmitRequest : Command<InsuranceVerification>
        {
            public VerificationRequest Request { get; set; }
        }
    }
}
