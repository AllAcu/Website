using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class SubmittedForApproval : Event<InsuranceVerification>
        {
            public Guid AssignedToUserId { get; set; }
            public string Comments { get; set; }
            public string Result { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Status = VerificationRequestStatus.PendingApproval;
                verification.AssignedTo = AssignedToUserId;
            }
        }
    }
}