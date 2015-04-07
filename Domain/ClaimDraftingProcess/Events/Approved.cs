using System.Diagnostics;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class Approved : Event<ClaimFilingProcess>
        {
            public override void Update(ClaimFilingProcess aggregate)
            {
                Debug.WriteLine("Approved that claim");
            }
        }
    }
}