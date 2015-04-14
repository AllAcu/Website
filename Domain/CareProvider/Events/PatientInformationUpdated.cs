using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class PatientInformationUpdated : Event<CareProvider>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public override void Update(CareProvider provider)
            {
                var patient = provider.GetPatient(Id);
                patient.Name = Name;
            }
        }
    }
}
