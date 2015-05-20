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
                UpdatedDateOfBirth = (command.DateOfBirth != patient.DateOfBirth) ? command.DateOfBirth : (DateTime?)null
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

        public void EnactCommand(Poke command)
        {
            RecordEvent(new Poked
            {
                PokeId = command.PokeId
            });
        }

        public void EnactCommand(UpdateVerificationRequestDraft command)
        {
            RecordEvent(new VerificationDraftUpdated
            {
                VerificationId = command.VerificationId,
                Request = command.RequestDraft
            });
        }

        public void EnactCommand(StartVerificationRequestDraft command)
        {
            var verificationId = Guid.NewGuid();
            RecordEvent(new VerificationDraftCreated
            {
                PatientId = command.PatientId,
                VerificationId = verificationId
            });

            RecordEvent(new VerificationDraftUpdated
            {
                VerificationId = verificationId,
                Request = command.RequestDraft
            });
        }

        public void EnactCommand(SubmitVerificationRequest command)
        {
            var verificationId = command.VerificationId ?? Guid.NewGuid();
            if (command.VerificationId == null)
            {
                RecordEvent(new VerificationDraftCreated
                {
                    VerificationId = verificationId
                });
            }

            if (command.VerificationRequest != null)
            {
                RecordEvent(new VerificationDraftUpdated
                {
                    VerificationId = verificationId,
                    Request = command.VerificationRequest
                });
            }

            RecordEvent(new VerificationRequestSubmitted
            {
                VerificationId = verificationId
            });
        }
    }
}
