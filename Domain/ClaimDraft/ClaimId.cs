using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public class ClaimId : String<ClaimId>
    {
        public ClaimId(Guid id) : base(id.ToString())
        {
        }

        public static ClaimId New()
        {
            return new ClaimId(Guid.NewGuid());
        }
    }
}