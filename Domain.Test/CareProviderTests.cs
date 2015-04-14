using System;
using Domain.CareProvider;
using Domain.ClaimFiling;
using Microsoft.Its.Domain;
using Xunit;

namespace Domain.Test
{
    public class CareProviderTests
    {
        public CareProviderTests()
        {
            Command<CareProvider.CareProvider>.AuthorizeDefault = (provider, command) => true;
            Command<ClaimFilingProcess>.AuthorizeDefault = (provider, command) => true;
        }

        [Fact]
        public void IntakeOfPatient_AddsPatientToCareProvider()
        {
            var provider = new CareProvider.CareProvider();

            var command = new CareProvider.CareProvider.IntakePatient
            {
                Name = "Phillip"
            };
            command.ApplyTo(provider);

            Assert.Contains(provider.Patients, p => p.Name == "Phillip");
        }

        [Fact]
        public void InitiatingClaim_AddsToProviderCollection()
        {
            var provider = new CareProvider.CareProvider();

            var draft = new ClaimDraft();

            var command = new CareProvider.CareProvider.StartClaim
            {
                Claim = draft
            };
            command.ApplyTo(provider);

            Assert.Contains(provider.Drafts, d => d == draft);
        }

        [Fact]
        public void CannotUpdateInsuranceOfNonPatient()
        {
            var provider = new CareProvider.CareProvider(Guid.NewGuid());

            var command = new CareProvider.CareProvider.UpdateInsurance
            {
                PatientId = Guid.NewGuid()
            };

            Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider));
        }

        [Fact]
        public void CannotTerminateInsuranceOfNonPatient()
        {
            var provider = new CareProvider.CareProvider(Guid.NewGuid());

            var command = new CareProvider.CareProvider.TerminateInsurance
            {
                PatientId = Guid.NewGuid()
            };

            Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider));
        }

        [Fact]
        public void CannotUpdatePatientInformationOfNonPatient()
        {
            var provider = new CareProvider.CareProvider(Guid.NewGuid());

            var command = new CareProvider.CareProvider.UpdatePatientInformation
            {
                PatientId = Guid.NewGuid()
            };

            Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider));
        }
    }
}
