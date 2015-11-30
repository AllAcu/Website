using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class StartCall : Command<InsuranceVerification>
        {
            public string ServiceCenterRepresentative { get; set; }
            public DateTimeOffset? TimeStarted { get; set; }
        }
    }
}
