using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Complete : Command<InsuranceVerification>
        {
            public Guid ApproverUserId { get; set; }
            public string Comments { get; set; }
        }
    }
}
