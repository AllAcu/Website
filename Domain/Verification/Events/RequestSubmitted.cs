using Domain.Verification;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class RequestSubmitted : Event<InsuranceVerification>
        {
            public override void Update(InsuranceVerification verification)
            {
                verification.Status = VerificationRequestStatus.Submitted;
            }
        }
    }
}
