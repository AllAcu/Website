using Domain.CareProvider;
using Xunit;

namespace Domain.Test
{
    public class CareProviderTests
    {
        [Fact]
        public void IntakeOfPatient_AddsPatientToCareProvider()
        {
            var provider = new CareProvider.CareProvider();

            var patient = new Patient();
            provider.EnactCommand(new CareProvider.CareProvider.IntakePatient
            {
                Patient = patient
            });

            Assert.Contains(provider.Patients, p => p == patient);
        }

        [Fact]
        public void InitiatingClaim_AddsToProviderCollection()
        {
            var provider = new CareProvider.CareProvider();

            var draft = new ClaimDraft();
            provider.EnactCommand(new CareProvider.CareProvider.StartClaim
            {
                Claim = draft
            });

            Assert.Contains(provider.Drafts, d => d == draft);
        }

    }
}
