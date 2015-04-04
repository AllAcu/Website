using System.Collections.Generic;
using Domain;

namespace Api.Models
{
    public class ClaimsIndex
    {
        public IList<ClaimDraft> Claims { get; set; }
    }
}