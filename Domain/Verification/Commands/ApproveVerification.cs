using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class ApproveVerification : Command<InsuranceVerification>
        {
            public Benefits Benefits { get; set; }
            public InsuranceCompanyCall ApprovalCall { get; set; }
        }
    }
}
