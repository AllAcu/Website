using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class InsuranceTerminated : Event<CareProvider>
        {
            public Guid PatientId { get; set; }
            public DateTimeOffset TerminationDate { get; set; }

            public override void Update(CareProvider provider)
            {
                var patient = provider.GetPatient(PatientId);
                patient.InsurancePolicies.Last().TerminationDate = TerminationDate;
            }
        }
    }
}
