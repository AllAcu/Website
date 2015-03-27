using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Patient
    {
        public PatientId Id { get; set; }
        public string Name { get; set; }
        public string Insurance { get; set; }

        public class PatientId : String<PatientId>
        {
            
        }
    }
}