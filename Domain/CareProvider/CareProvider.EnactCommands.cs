namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public void EnactCommand(StartClaim command)
        {
            command.Claim.Id = Id;
            RecordEvent(new ClaimStarted(command.Claim));
        }

        public void EnactCommand(UpdateClaimDraft command)
        {
            RecordEvent(new ClaimUpdated(command.Claim));
        }

        public void EnactCommand(IntakePatient command)
        {
            RecordEvent(new NewPatient
            {
                Name = command.Name
            });
        }

        public void EnactCommand(UpdateInsurance command)
        {
            RecordEvent(new InsuranceUpdated
            {
                InsuranceCompany = command.InsuranceCompany,
                Plan = command.Plan,
                EffectiveDate = command.EffectiveDate,
                PolicyDate = command.PolicyDate,
                IssueDate = command.IssueDate
            });
        }

        public void EnactCommand(TerminateInsurance command)
        {
            RecordEvent(new InsuranceTerminated
            {
                PatientId = command.PatientId,
                TerminationDate = command.TerminationDate
            });
        }
    }
}
