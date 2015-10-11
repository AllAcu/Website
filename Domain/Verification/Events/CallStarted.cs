using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class CallStarted : Event<InsuranceVerification>
        {
            public string ServiceCenterRepresentative { get; set; }
            public DateTimeOffset TimeStarted { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Status = VerificationRequestStatus.InProgress;
            }
        }
    }
}
