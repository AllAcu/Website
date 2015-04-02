using System;
using System.Diagnostics;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Approved : Event<ClaimFilingProcess>
    {
        public override void Update(ClaimFilingProcess aggregate)
        {
            Debug.WriteLine("Approved that claim");
        }
    }
}