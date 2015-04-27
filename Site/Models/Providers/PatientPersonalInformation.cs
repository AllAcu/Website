using System;
using Domain;

namespace AllAcu.Models.Providers
{
    public class PatientPersonalInformation
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        //public Address Address { get; set; }
    }
}