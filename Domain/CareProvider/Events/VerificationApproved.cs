using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class VerificationApproved : Event<CareProvider>
        {
            public Guid VerificationId { get; set; }
            public Benefits Benefits { get; set; } // TODO (bremor) - want this in the event?

            public override void Update(CareProvider provider)
            {
                var verification = provider.PendingVerifications.Single(p => p.Id == VerificationId);
                provider.PendingVerifications.Remove(verification);

                var patient = provider.GetPatient(verification.PatientId);
                patient.InsurancePolicies.Last().Benefits = verification.Benefits;
            }
        }
    }
}
