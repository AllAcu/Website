using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class ReturnToProvider : Command<InsuranceVerification>
        {
            public string Reason { get; set; }
        }
    }
}
