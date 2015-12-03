using System;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class TerminateInsurance : Command<CareProvider>
        {
            public Guid PatientId { get; set; }
            public DateTimeOffset TerminationDate { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get { return Validate.That<CareProvider>(provider => provider.PatientExists(PatientId)); }
            }
        }
    }
}
