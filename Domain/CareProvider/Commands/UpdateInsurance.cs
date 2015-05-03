using System;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdateInsurance : Command<CareProvider>
        {
            public Guid PatientId { get; set; }

            public MedicalInsurance MedicalInsurance { get; set; }
            public PersonalInjuryProtection PersonalInjuryProtection { get; set; }

            public bool SecondaryCoverage { get; set; }

            public override IValidationRule CommandValidator
            {
                get
                {
                    return new ValidationPlan<UpdateInsurance>(
                        Validate.That<UpdateInsurance>(
                            cmd => cmd.MedicalInsurance != null || cmd.PersonalInjuryProtection != null),
                        Validate.That<UpdateInsurance>(
                            cmd => !(cmd.MedicalInsurance != null && cmd.PersonalInjuryProtection != null))
                        ).WithErrorMessage("Either medical or personal injury protection must be supplied");
                }
            }

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
