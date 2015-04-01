using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimDraft
    {
        public class Create : Command<ClaimDraft>
        {
            public string Diagnosis { get; set; }
        }
    }
}
