using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class StartCall : Command<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }
            public InsuranceCompanyCall ApprovalCall { get; set; }
        }
    }
}
