using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class NewPatient : Event<CareProvider>
        {
            public Patient Patient { get; set; }
            public override void Update(CareProvider provider)
            {
                provider.Patients.Add(Patient);
            }
        }
    }
}
