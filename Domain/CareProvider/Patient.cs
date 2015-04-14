using System;
using System.Collections.Generic;

namespace Domain
{
    public class Patient
    {
        public Patient(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();
    }
}