using System;

namespace AllAcu.Models.Providers
{
    public class PatientEditViewModel
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}
