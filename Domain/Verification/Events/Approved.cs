using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Approved : Event<InsuranceVerification>
        {
            public override void Update(InsuranceVerification verification)
            {
                verification.Status = PendingVerification.RequestStatus.Approved;

                //var patient = provider.GetPatient(verification.PatientId);
                //patient.InsurancePolicies.Last().Benefits = verification.Benefits;
            }
        }
    }
}
