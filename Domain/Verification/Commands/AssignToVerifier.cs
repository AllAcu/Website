using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class AssignToVerifier : Command<InsuranceVerification>
        {
            public Guid UserId { get; set; }
        }
    }
}
