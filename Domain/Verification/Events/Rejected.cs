using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Rejected : Event<InsuranceVerification>
        {
            public string Reason { get; set; }

            public override void Update(InsuranceVerification verification)
            {
                verification.Status = PendingVerification.RequestStatus.Draft;
            }
        }
    }
}
