using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class CreateVerification : ConstructorCommand<InsuranceVerification>
        {
            public Guid PatientId { get; set; }
            public VerificationRequest RequestDraft { get; set; }
        }
    }
}
