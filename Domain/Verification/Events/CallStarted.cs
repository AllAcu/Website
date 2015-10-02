﻿using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class CallStarted : Event<InsuranceVerification>
        {
            public Guid PatientId { get; set; }
            public VerificationRequest Request { get; set; }

            public override void Update(InsuranceVerification verification)
            {
            }
        }
    }
}
