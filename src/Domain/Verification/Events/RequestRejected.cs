using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class RequestRejected : Event<InsuranceVerification>
        {
            public string Reason { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Status = VerificationRequestStatus.Rejected;
            }
        }
    }
}
