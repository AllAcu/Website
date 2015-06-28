using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class Assigned : Event<InsuranceVerification>
        {
            public Guid UserId { get; set; }
            public override void Update(InsuranceVerification aggregate)
            {
                
            }
        } 
    }
}
