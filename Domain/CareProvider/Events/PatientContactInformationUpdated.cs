using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider.Events
{
    public class PatientContactInformationUpdated : Event<CareProvider>
    {
        public Guid PatientId { get; set; }
        public Address Address { get; set; }

        public override void Update(CareProvider provider)
        {
            var patient = provider.GetPatient(PatientId);

            patient.Address = Address;
        }
    }
}
