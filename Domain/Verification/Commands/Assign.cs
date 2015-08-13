using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Assign : Command<InsuranceVerification>
        {
            public Guid UserId { get; set; }
            public string Comments { get; set; }
        }
    }
}
