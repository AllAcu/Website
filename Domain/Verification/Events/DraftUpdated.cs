using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class DraftUpdated : Event<InsuranceVerification>
        {
            public VerificationRequest Request { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Request = Request;
            }
        }
    }
}
