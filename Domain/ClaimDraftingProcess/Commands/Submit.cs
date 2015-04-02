using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class SubmitForApproval : Command<ClaimFilingProcess>
        {
        }
    }
}