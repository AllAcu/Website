using System;
using System.Collections.Generic;

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
        public PhoneNumber PhoneNumber { get; set; }

        public DateTimeOffset DateOfBirth
        {
            get { return _dateOfBirth.Date; }
            set { _dateOfBirth = value.Date; }
        }

        public IList<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();
    }
}