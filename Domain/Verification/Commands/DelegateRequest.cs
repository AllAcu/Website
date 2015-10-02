using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class DelegateRequest : Command<InsuranceVerification>
        {
            public Guid AssignedToUserId { get; set; }
            public string Comments { get; set; }
        }
    }
}
