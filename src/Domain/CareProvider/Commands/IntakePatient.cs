using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class IntakePatient : Command<CareProvider>
        {
            public Guid PatientId { get; } = Guid.NewGuid();
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public City City { get; set; }
            public State State { get; set; }
            public PostalCode PostalCode { get; set; }
            public Gender Gender { get; set; }
            public PhoneNumber PhoneNumber { get; set; }
        }
    }
}
