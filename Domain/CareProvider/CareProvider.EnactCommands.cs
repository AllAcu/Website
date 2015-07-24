using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Serialization;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class CareProviderCommandHandler :
            ICommandHandler<CareProvider, WelcomeUser>,
            ICommandHandler<CareProvider, DismissUser>,
            ICommandHandler<CareProvider, UpdateProvider>,
            ICommandHandler<CareProvider, StartClaim>,
            ICommandHandler<CareProvider, UpdateClaimDraft>,
            ICommandHandler<CareProvider, IntakePatient>,
            ICommandHandler<CareProvider, UpdatePatientPersonalInformation>,
            ICommandHandler<CareProvider, UpdatePatientContactInformation>,
            ICommandHandler<CareProvider, UpdateInsurance>,
            ICommandHandler<CareProvider, TerminateInsurance>
        {
            public async Task EnactCommand(CareProvider provider, WelcomeUser command)
            {
                provider.RecordEvent(new UserJoined
                {
                    UserId = command.UserId
                });
            }

            public async Task EnactCommand(CareProvider provider, DismissUser command)
            {
                provider.RecordEvent(new UserLeft
                {
                    UserId = command.UserId
                });
            }

            public async Task EnactCommand(CareProvider provider, UpdateProvider command)
            {
                provider.RecordEvent(new ProviderUpdated
                {
                    BusinessName = command.BusinessName,
                    City = command.City,
                    NpiNumber = command.NpiNumber,
                    TaxId = command.TaxId
                });
            }

            public async Task EnactCommand(CareProvider provider, StartClaim command)
            {
                provider.RecordEvent(new ClaimStarted
                {
                    Draft = new ClaimDraft
                    {
                        Patient = provider.GetPatient(command.PatientId),
                        Visit = command.Visit
                    }
                });
            }

            public async Task EnactCommand(CareProvider provider, UpdateClaimDraft command)
            {
                provider.RecordEvent(new ClaimUpdated(provider.ClaimDrafts.SingleOrDefault(d => d.Id == command.ClaimDraftId)));
            }

            public async Task EnactCommand(CareProvider provider, IntakePatient command)
            {
                provider.RecordEvent(new NewPatient
                {
                    PatientId = command.PatientId,
                    Name = command.Name,
                    Gender = command.Gender,
                    DateOfBirth = command.DateOfBirth,
                    Address = new Address
                    {
                        Line1 = command.Address1,
                        Line2 = command.Address2,
                        City = command.City,
                        State = command.State,
                        PostalCode = command.PostalCode
                    }
                });
            }

            public async Task EnactCommand(CareProvider provider, UpdatePatientPersonalInformation command)
            {
                var patient = provider.GetPatient(command.PatientId);
                provider.RecordEvent(new PatientInformationUpdated
                {
                    PatientId = command.PatientId,
                    UpdatedName = (command.Name != patient.Name) ? command.Name : null,
                    UpdatedGender = (command.Gender != patient.Gender) ? command.Gender : null,
                    UpdatedDateOfBirth =
                        (command.DateOfBirth != patient.DateOfBirth) ? command.DateOfBirth : (DateTime?) null
                });
            }

            public async Task EnactCommand(CareProvider provider, UpdatePatientContactInformation command)
            {
                var patient = provider.GetPatient(command.PatientId);
                provider.RecordEvent(new PatientContactInformationUpdated
                {
                    PatientId = command.PatientId,
                    UpdatedAddress = (command.Address != patient.Address) ? command.Address : null,
                    UpdatedPhoneNumber = (command.PhoneNumber != patient.PhoneNumber) ? command.PhoneNumber : null
                });
            }

            public async Task EnactCommand(CareProvider provider, UpdateInsurance command)
            {
                provider.RecordEvent(new InsuranceUpdated
                {
                    PatientId = command.PatientId,
                    PersonalInjuryProtection = command.PersonalInjuryProtection,
                    MedicalInsurance = command.MedicalInsurance
                });
            }

            public async Task EnactCommand(CareProvider provider, TerminateInsurance command)
            {
                provider.RecordEvent(new InsuranceTerminated
                {
                    PatientId = command.PatientId,
                    TerminationDate = command.TerminationDate
                });
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<WelcomeUser> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<DismissUser> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<UpdateProvider> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<StartClaim> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<UpdateClaimDraft> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<IntakePatient> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<UpdatePatientPersonalInformation> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<UpdatePatientContactInformation> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<UpdateInsurance> command)
            {
            }

            public async Task HandleScheduledCommandException(CareProvider provider, CommandFailed<TerminateInsurance> command)
            {
            }
        }
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
