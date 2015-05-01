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
            public string InsuranceCompany { get; set; }
            public string Plan { get; set; }
            public PhoneNumber ProviderNumber { get; set; }
            public InsuranceId InsuranceId { get; set; }
            public GroupNumber GroupNumber { get; set; }

            public DateTimeOffset IssueDate { get; set; }
            public DateTimeOffset PolicyDate { get; set; }
            public DateTimeOffset EffectiveDate { get; set; }

            public bool SecondaryCoverage { get; set; }

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
