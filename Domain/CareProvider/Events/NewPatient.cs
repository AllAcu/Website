using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class NewPatient : Event<CareProvider>
        {
            public string Name { get; set; }
            public Guid PatientId { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Gender { get; set; }
            public Address Address { get; set; }
            public PhoneNumber PhoneNumber { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.Patients.Add(new Patient(PatientId)
                {
                    Name = Name,
                    DateOfBirth = DateOfBirth,
                    Gender = Gender,
                    Address = Address,
                    PhoneNumber = PhoneNumber
                });
            }
        }
    }
}
