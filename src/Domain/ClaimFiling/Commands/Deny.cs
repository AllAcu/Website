using System;
using Microsoft.Its.Domain;

namespace Domain.ClaimFiling
{
    public partial class ClaimFilingProcess
    {
        public class Deny : Command<ClaimFilingProcess>
        {
        }
    }
}