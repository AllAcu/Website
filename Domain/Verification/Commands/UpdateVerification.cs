using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class UpdateVerification : Command<InsuranceVerification>
        {
            public Guid VerificationId { get; set; }
            public Benefits Benefits { get; set; }
        }
    }
}
