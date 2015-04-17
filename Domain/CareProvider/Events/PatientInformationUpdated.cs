using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class PatientInformationUpdated : Event<CareProvider>
        {
            public Guid PatientId { get; set; }
            public string UpdatedName { get; set; }
            public DateTimeOffset? UpdatedDateOfBirth { get; set; }
            public Gender UpdatedGender { get; set; }

            public override void Update(CareProvider provider)
            {
                var patient = provider.GetPatient(PatientId);
                patient.Name = UpdatedName ?? patient.Name;
                patient.DateOfBirth = UpdatedDateOfBirth ?? patient.DateOfBirth;
                patient.Gender = UpdatedGender ?? patient.Gender;
            }
        }
    }
}
