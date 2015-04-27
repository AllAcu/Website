using System;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdatePatientPersonalInformation : Command<CareProvider>
        {
            public Guid PatientId { get; set; }
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Gender Gender { get; set; }

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
