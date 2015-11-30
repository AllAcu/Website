using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Delegated : Event<InsuranceVerification>
        {
            public Guid AssignedToUserId { get; set; }
            public string Comments { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.AssignedTo = AssignedToUserId;
            }
        }
    }
}
