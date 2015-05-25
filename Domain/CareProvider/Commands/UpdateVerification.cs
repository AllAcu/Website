using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdateVerification
        {
            public Guid VerificationId { get; set; }
            Benefits Benefits { get; set; }
        }
    }
}
