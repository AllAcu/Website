using System;
using Microsoft.Its.Domain;

namespace Domain.ClaimFiling
{
    public partial class ClaimFilingProcess
    {
        public class Approve : Command<ClaimFilingProcess>
        {
            public Guid Approver { get; set; }
        }
    }
}