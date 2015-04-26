using System;

namespace AllAcu.Models.Providers
{
    public class PatientEditViewModel
    {
        public Guid PatientId { get; set; }
        public PatientPersonalInformation PersonalInformation { get; set; }
    }
}
