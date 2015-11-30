using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class RejectRequest : Command<InsuranceVerification>
        {
            public string Comments { get; set; }
        }
    }
}
