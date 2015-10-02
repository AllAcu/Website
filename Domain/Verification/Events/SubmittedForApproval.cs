using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class SubmittedForApproval : Event<InsuranceVerification>
        {
            public override void Update(InsuranceVerification verification)
            {
                
            }
        }
    }
}