using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationUpdated : Event<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }

            public override void Update(InsuranceVerification provider)
            {
                //var existingVerification = provider.PendingVerifications.First(v => v.Id == VerificationId);
                //existingVerification.Benefits = Benefits;
            }
        }
    }
}
