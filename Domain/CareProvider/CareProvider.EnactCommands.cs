namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public void EnactCommand(StartClaim command)
        {
            command.Claim.Id = Id;
            RecordEvent(new ClaimStarted(command.Claim));
        }

        public void EnactCommand(UpdateClaim command)
        {
            RecordEvent(new ClaimUpdated(command.Claim));
        }

        public void EnactCommand(IntakePatient command)
        {
            RecordEvent(new NewPatient
            {
                Patient = command.Patient
            });
        }

        public void EnactCommand(CreateProvider command)
        {
            RecordEvent(new NewProvider
            {
                BusinessName = command.BusinessName,
                PractitionerName = command.PractitionerName,
                City = command.City
            });
        }
    }
}
