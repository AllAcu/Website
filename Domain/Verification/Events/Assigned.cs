using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Assigned : Event<InsuranceVerification>
        {
            public Guid UserId { get; set; }
            public string Comments { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Assignment = new VerificationAssignment
                {
                    UserId = UserId,
                    Comments = Comments
                };
            }
        }
    }
}
