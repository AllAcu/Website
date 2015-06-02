using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationRequestSubmitted : Event<InsuranceVerification>
        {
            public override void Update(InsuranceVerification provider)
            {
                //var draft = provider.PendingVerifications.Single(d => d.Id == VerificationId);
                //draft.Status = PendingVerification.RequestStatus.Submitted;
            }
        }
    }
}
