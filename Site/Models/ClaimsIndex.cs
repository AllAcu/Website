using System.Collections.Generic;
using Domain;
using Domain.CareProvider;

namespace AllAcu.Models
{
    public class ClaimsIndex
    {
        public IList<ClaimDraft> Claims { get; set; }
    }
}