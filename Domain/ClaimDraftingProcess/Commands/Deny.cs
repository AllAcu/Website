using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class Deny : Command<ClaimFilingProcess>
        {
        }
    }
}