using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public class SubmittedToInsurance : Event<Claim>
    {
        public override void Update(Claim aggregate)
        {
            throw new NotImplementedException();
        }
    }
}