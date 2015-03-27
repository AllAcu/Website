using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimDraft
    {
        public class Approve : Command<ClaimDraft>
        {
            public User Approver { get; set; }
        }
    }
}