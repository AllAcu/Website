using System;

namespace Domain
{
    public partial class Claim
    {
        public void EnactCommand(CheckStatus command)
        {
            RecordEvent(new StatusChecked());
        }
        public void EnactCommand(SubmitToInsurance command)
        {
            RecordEvent(new SubmittedToInsurance());
        }
    }
}