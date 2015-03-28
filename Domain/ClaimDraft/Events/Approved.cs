using System;
using System.Diagnostics;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Approved : Event<ClaimDraft>
    {
        public override void Update(ClaimDraft aggregate)
        {
            Debug.WriteLine("Approved that claim");
        }
    }
}