using System;
using Domain;

namespace AllAcu.Models.Patients
{
    public class PatientPersonalInformation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Address Address { get; set; }
    }
}