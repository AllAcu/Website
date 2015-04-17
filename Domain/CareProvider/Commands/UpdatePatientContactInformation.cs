using System;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider.Commands
{
    public class UpdatePatientContactInformation : Command<CareProvider>
    {
        public Guid PatientId { get; set; }
        public Address Address { get; set; }

        public override IValidationRule<CareProvider> Validator
        {
            get { return Validate.That<CareProvider>(c => c.PatientExists(PatientId)); }
        }
    }
}
