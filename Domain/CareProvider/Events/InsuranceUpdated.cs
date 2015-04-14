using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class InsuranceUpdated : Event<CareProvider>
        {
            public Guid PatientId { get; set; }
            public string InsuranceCompany { get; set; }
            public string Plan { get; set; }
            public DateTimeOffset IssueDate { get; set; }
            public DateTimeOffset PolicyDate { get; set; }
            public DateTimeOffset EffectiveDate { get; set; }

            public override void Update(CareProvider provider)
            {
                var patient = provider.GetPatient(PatientId);
                patient.InsurancePolicies.Add(new InsurancePolicy
                {
                    InsuranceCompany = InsuranceCompany,
                    Plan = Plan,
                    EffectiveDate = EffectiveDate,
                    PolicyDate = PolicyDate,
                    IssueDate = IssueDate
                });
            }
        }
    }
}
