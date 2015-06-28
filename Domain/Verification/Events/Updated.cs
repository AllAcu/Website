using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Updated : Event<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }

            public override void Update(InsuranceVerification provider)
            {
                //var existingVerification = provider.PendingVerifications.First(v => v.Id == VerificationId);
                //existingVerification.Benefits = Benefits;
            }
        }
    }
}
