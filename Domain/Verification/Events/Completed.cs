using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Completed : Event<InsuranceVerification>
        {
            public Guid ApproverUserId { get; set; }
            public string Comments { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Status = VerificationRequestStatus.Approved;

                //var patient = provider.GetPatient(verification.PatientId);
                //patient.InsurancePolicies.Last().Benefits = verification.Benefits;
            }

        }
    }
}
