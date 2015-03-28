using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Denied : Event<ClaimDraft>
    {
        public override void Update(ClaimDraft aggregate)
        {
            throw new NotImplementedException();
        }
    }
}