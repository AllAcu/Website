using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class CallEnded : Event<InsuranceVerification>
        {
            public override void Update(InsuranceVerification aggregate)
            {
            }
        }
    }
}
