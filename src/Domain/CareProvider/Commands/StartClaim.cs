using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class StartClaim : Command<CareProvider>
        {
            public Guid PatientId { get; set; }
            public Visit Visit { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(provider =>
                        provider.Patients.SingleOrDefault(patient => patient.Id == PatientId) != null);
                }
            }
        }
    }
}
