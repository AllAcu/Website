using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationApproved : Event<InsuranceVerification>
        {
            public Guid VerificationId { get; set; }

            public override void Update(InsuranceVerification provider)
            {
                //var verification = provider.PendingVerifications.Single(p => p.Id == VerificationId);
                //provider.PendingVerifications.Remove(verification);

                //var patient = provider.GetPatient(verification.PatientId);
                //patient.InsurancePolicies.Last().Benefits = verification.Benefits;
            }
        }
    }
}
