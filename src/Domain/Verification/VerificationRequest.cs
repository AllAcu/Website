namespace Domain.Verification
{
    public class VerificationRequest
    {
        public string Specialty { get; set; }
        public string Provider { get; set; }
        public bool IsInNetwork { get; set; }
        public bool IsAcupuncture { get; set; }
        public bool IsTuaNa { get; set; }
        public bool IsHeatLamp { get; set; }
        public bool IsTheraputicExcercises { get; set; }
        public bool IsChiropractic { get; set; }
        public bool IsMassageTherapy { get; set; }
        public string Comments { get; set; }
    }
}