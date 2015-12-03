using Microsoft.Its.Domain;

namespace Domain.ClaimFiling
{
    public partial class ClaimFilingProcess
    {
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