using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class SubmitForApproval : Command<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }
            public string Comments { get; set; }
        }
    }
}