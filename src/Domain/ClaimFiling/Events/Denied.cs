﻿using System;
using Microsoft.Its.Domain;

namespace Domain.ClaimFiling
{
    public partial class ClaimFilingProcess
    {
        public class Denied : Event<ClaimFilingProcess>
        {
            public override void Update(ClaimFilingProcess aggregate)
            {
                throw new NotImplementedException();
            }
        }
    }
}