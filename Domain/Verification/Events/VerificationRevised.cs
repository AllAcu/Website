using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationRevised : Event<InsuranceVerification>
        {
            public string Reason { get; set; }

            public override void Update(InsuranceVerification provider)
            {
                //var verification = provider.PendingVerifications.Single(p => p.Id == VerificationId);
                //verification.Status = PendingVerification.RequestStatus.Draft;
            }
        }
    }
}
