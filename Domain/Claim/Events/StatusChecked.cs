using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public class StatusChecked : Event<Claim>
    {
        public override void Update(Claim aggregate)
        {
            throw new NotImplementedException();
        }
    }
}