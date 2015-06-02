﻿using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationDraftUpdated : Event<InsuranceVerification>
        {
            public VerificationRequest Request { get; set; }

            public override void Update(InsuranceVerification provider)
            {
                //provider.PendingVerifications.Single(r => r.Id == VerificationId)
                //    .Request = Request;
            }
        }
    }
}
