using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerifyBenefits : Command<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }
            public InsuranceCompanyCall ApprovalCall { get; set; }
        }
    }
}
