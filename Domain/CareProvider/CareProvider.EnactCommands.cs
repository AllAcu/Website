using System;
using System.Linq;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public void EnactCommand(StartClaim command)
        {
            RecordEvent(new ClaimStarted
            {
                Draft = new ClaimDraft
                {
                    Patient = GetPatient(command.PatientId),
                    Visit = command.Visit
                }
            });
        }

        public void EnactCommand(UpdateClaimDraft command)
        {
            RecordEvent(new ClaimUpdated(ClaimDrafts.SingleOrDefault(d => d.Id == command.ClaimDraftId)));
        }

        public void EnactCommand(IntakePatient command)
        {
            RecordEvent(new NewPatient
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

        public void EnactCommand(UpdatePatientPersonalInformation command)
        {
            var patient = GetPatient(command.PatientId);
            RecordEvent(new PatientInformationUpdated
            {
                PatientId = command.PatientId,
                UpdatedName = (command.Name != patient.Name) ? command.Name : null,
                UpdatedGender = (command.Gender != patient.Gender) ? command.Gender : null,
                UpdatedDateOfBirth =
                    (command.DateOfBirth != patient.DateOfBirth) ? command.DateOfBirth : (DateTime?)null
            });
        }

        public void EnactCommand(UpdatePatientContactInformation command)
        {
            var patient = GetPatient(command.PatientId);
            RecordEvent(new PatientContactInformationUpdated
            {
                PatientId = command.PatientId,
                UpdatedAddress = (command.Address != patient.Address) ? command.Address : null,
                UpdatedPhoneNumber = (command.PhoneNumber != patient.PhoneNumber) ? command.PhoneNumber : null
            });
        }

        public void EnactCommand(UpdateInsurance command)
        {
            RecordEvent(new InsuranceUpdated
            {
                PatientId = command.PatientId,
                PersonalInjuryProtection = command.PersonalInjuryProtection,
                MedicalInsurance = command.MedicalInsurance
            });
        }

        public void EnactCommand(TerminateInsurance command)
        {
            RecordEvent(new InsuranceTerminated
            {
                PatientId = command.PatientId,
                TerminationDate = command.TerminationDate
            });
        }

        public void EnactCommand(WelcomeUser command)
        {
            RecordEvent(new UserJoined
            {
                UserId = command.UserId
            });
        }

        public void EnactCommand(DismissUser command)
        {
            RecordEvent(new UserLeft
            {
                UserId = command.UserId
            });
        }
    }
}
