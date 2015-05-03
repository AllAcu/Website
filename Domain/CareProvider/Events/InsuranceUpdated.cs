using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class InsuranceUpdated : Event<CareProvider>
        {
            public Guid PatientId { get; set; }

            public MedicalInsurance MedicalInsurance { get; set; }
            public PersonalInjuryProtection PersonalInjuryProtection { get; set; }

            public override void Update(CareProvider provider)
            {
                var patient = provider.GetPatient(PatientId);

                var policy = MedicalInsurance != null ? new InsurancePolicy<MedicalInsurance>(MedicalInsurance)
                    : PersonalInjuryProtection != null ? new InsurancePolicy<PersonalInjuryProtection>(PersonalInjuryProtection)
                        : (InsurancePolicy)null;

                if (policy != null)
                {
                    patient.InsurancePolicies.Add(policy);
                }
            }
        }
    }
}
