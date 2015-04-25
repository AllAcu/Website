using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Patient
    {
        private DateTimeOffset _dateOfBirth;

        public Patient(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public Address Address { get; set; }

        public DateTimeOffset DateOfBirth
        {
            get { return _dateOfBirth.Date; }
            set { _dateOfBirth = value.Date; }
        }

        public IList<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();
    }


    public class Gender : String<Gender>
    {
        public Gender(string value) : base(value)
        {
            
        }

        public static implicit operator Gender(string gender)
        {
            return new Gender(gender);
        }

        public static Gender Male = new Gender("Male");
        public static Gender Female = new Gender("Female`");
    }
}