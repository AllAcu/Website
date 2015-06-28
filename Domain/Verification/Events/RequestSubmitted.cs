﻿using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class RequestSubmitted : Event<InsuranceVerification>
        {
            public override void Update(InsuranceVerification verification)
            {
                verification.Status = PendingVerification.RequestStatus.Submitted;
            }
        }
    }
}
