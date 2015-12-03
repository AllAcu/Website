﻿using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Delegate : Command<InsuranceVerification>
        {
            public Guid AssignToUserId { get; set; }
            public string Comments { get; set; }
        }
    }
}
