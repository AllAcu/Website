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
        public class VerificationUpdated : Event<CareProvider>
        {
            public Guid VerificationId { get; set; }
            public Benefits Benefits { get; set; }

            public override void Update(CareProvider provider)
            {
                var existingVerification = provider.PendingVerifications.First(v => v.Id == VerificationId);
                existingVerification.Benefits = Benefits;
            }
        }
    }
}
