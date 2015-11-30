using System;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.ClaimFiling
{
    public partial class ClaimFilingProcess
    {
        public class SubmitForApproval : Command<ClaimFilingProcess>
        {
            public override IValidationRule<ClaimFilingProcess> Validator
            {
                get { return Validate.That<ClaimFilingProcess>(p => !p.HasBeenSubmitted);  }
            }
        }
    }
}