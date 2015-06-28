using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Complete : Command<InsuranceVerification>
        {
            public string Notes { get; set; }
        }
    }
}