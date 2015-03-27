using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimDraft
    {
        public class SubmitForApproval : Command<ClaimDraft>
        {
        }
    }
}