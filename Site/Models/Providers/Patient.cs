using System;
using AllAcu.Models.Providers;

namespace AllAcu
{
    public class Patient
    {
        public Guid PatientId { get; set; }
        public Guid ProviderId { get; set; }
        public PatientPersonalInformation PersonalInfo { get; set; } = new PatientPersonalInformation();
        public PatientContactInformation ContactInfo { get; set; } = new PatientContactInformation();
    }
}