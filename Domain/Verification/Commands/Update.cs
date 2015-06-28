using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Update : Command<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }
        }
    }
}
