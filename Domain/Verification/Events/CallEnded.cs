using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class CallEnded : Event<InsuranceVerification>
        {
            public override void Update(InsuranceVerification verification)
            {
            }

            public string ServiceCenterRepresentative { get; set; }
            public string ReferenceNumber { get; set; }
            public DateTimeOffset TimeEnded { get; set; }
            public string Result { get; set; }
            public string Comments { get; set; }
        }
    }
}
