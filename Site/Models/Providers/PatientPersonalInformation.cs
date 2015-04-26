using System;
using Domain;

namespace AllAcu.Models.Providers
{
    public class PatientPersonalInformation
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Address Address { get; set; }
    }
}