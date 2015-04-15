using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class IntakePatient : Command<CareProvider>
        {
            public Guid PatientId { get; }
            public string Name { get; }

            public IntakePatient(string name)
            {
                Name = name;
                PatientId = Guid.NewGuid();
            }
        }
    }
}
