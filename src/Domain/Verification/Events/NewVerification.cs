using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class NewVerification : Event<InsuranceVerification>
        {
            public Guid PatientId { get; set; }
            public VerificationRequest RequestDraft { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.PatientId = PatientId;
                verification.Request = RequestDraft ?? new VerificationRequest();
                verification.Status = VerificationRequestStatus.Draft;
            }
        }
    }
}
