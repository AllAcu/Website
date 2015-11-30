using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Updated : Event<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Benefits = Benefits;
            }
        }
    }
}
