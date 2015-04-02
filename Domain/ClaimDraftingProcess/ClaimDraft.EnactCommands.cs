namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public void EnactCommand(StartClaim command)
        {
            RecordEvent(new ClaimInitiated(command.Claim));
        }

        public void EnactCommand(SubmitForApproval command)
        {
            RecordEvent(new Submitted());
        }

        public void EnactCommand(Approve command)
        {
            RecordEvent(new Approved());
        }

        public void EnactCommand(Deny command)
        {
            RecordEvent(new Denied());
        }
    }
}