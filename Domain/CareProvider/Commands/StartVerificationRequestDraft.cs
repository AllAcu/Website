using System;
using System.Linq;
using Domain.Verification;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class StartVerificationRequestDraft : Command<CareProvider>
        {
            public StartVerificationRequestDraft(VerificationRequest requestDraft)
            {
                RequestDraft = requestDraft;
            }

            public Guid PatientId { get; set; }
            public VerificationRequest RequestDraft { get; }

            public override IValidationRule CommandValidator
            {
                get
                {
                    return Validate.That<StartVerificationRequestDraft>(command => command.RequestDraft != null)
                        .WithMessage("Must supply a draft validation");
                }
            }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(provider => 
                        provider.GetPatient(PatientId).InsurancePolicies.Any(p => p.TerminationDate == null))
                        .WithMessage("Cannot start verification when there is no current insurance");
                }
            }
        }
    }
}
