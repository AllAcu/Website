using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ClaimDraft
    {
        public Patient Patient { get; set; }
        public CareProvider Provider { get; set; }
        public string Diagnosis { get; set; }
        public DateTimeOffset DateOfService { get; set; }
        public IList<Procedure> Procedures { get; set; }
        public Guid Id { get; set; }
    }
}
