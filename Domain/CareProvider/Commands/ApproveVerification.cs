using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class ApproveVerification : Command<CareProvider>
        {
            public Guid VerificationId { get; set; }
            public Benefits Benefits { get; set; }
        }
    }
}
