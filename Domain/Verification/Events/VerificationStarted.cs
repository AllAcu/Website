using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationStarted : Event<InsuranceVerification>
        {
            public Guid VerificationId { get; set; }
            public Guid PatientId { get; set; }

            public override void Update(InsuranceVerification aggregate)
            {
            }
        }
    }
}
