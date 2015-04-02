using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class Approve : Command<ClaimFilingProcess>
        {
            public User Approver { get; set; }
        }
    }
}