using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class EndCall : Command<InsuranceVerification>
        {
            public string ServiceCenterRepresentative { get; set; }
            public string ReferenceNumber { get; set; }
            public DateTimeOffset? TimeEnded { get; set; }
            public Benefits Benefits { get; set; }

            // results: verification done, rejected or unfound, interupted
            public string Result { get; set; }
            public string Comments { get; set; }
        }
    }
}
