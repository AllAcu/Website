using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdatePatientInformation : Command<CareProvider>
        {
            public Guid PatientId { get; set; } = Guid.NewGuid();
            public string Name { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(provider => provider.PatientExists(PatientId));
                }
            }
        }
    }
}
