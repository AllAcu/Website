using System;

namespace Domain
{
    public partial class ClaimDraft
    {
        public void EnactCommand(SubmitForApproval command)
        {
        }

        public void EnactCommand(Approve command)
        {
        }

        public void EnactCommand(Deny command)
        {
        }
    }
}