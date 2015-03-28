using System;

namespace Domain
{
    public partial class ClaimDraft
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