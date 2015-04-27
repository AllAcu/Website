using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class PatientContactInformationUpdated : Event<CareProvider>
        {
            public Guid PatientId { get; set; }
            public Address UpdatedAddress { get; set; }
            public Address UpdatedPhoneNumber { get; set; }

            public override void Update(CareProvider provider)
            {
                var patient = provider.GetPatient(PatientId);

                patient.Address = UpdatedAddress ?? patient.Address;
                patient.Address = UpdatedPhoneNumber ?? patient.PhoneNumber;
            }
        }
    }
}
