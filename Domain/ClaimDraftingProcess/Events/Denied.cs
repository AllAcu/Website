using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Denied : Event<ClaimFilingProcess>
    {
        public override void Update(ClaimFilingProcess aggregate)
        {
            throw new NotImplementedException();
        }
    }
}