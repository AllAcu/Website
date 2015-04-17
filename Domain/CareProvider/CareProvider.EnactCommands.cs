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
            RecordEvent(new ClaimUpdated(Drafts.SingleOrDefault(d => d.Id == command.ClaimDraftId)));
        }

        public void EnactCommand(IntakePatient command)
        {
            RecordEvent(new NewPatient
            {
                PatientId = command.PatientId,
                Name = command.Name
            });
        }

        public void EnactCommand(UpdatePatientInformation command)
        {
            var patient = GetPatient(command.PatientId);
            RecordEvent(new PatientInformationUpdated
            {
                PatientId = command.PatientId,
                UpdatedName = (command.Name != patient.Name) ? command.Name : null,
                UpdatedGender = (command.Gender != patient.Gender) ? command.Gender : null,
                UpdatedDateOfBirth = (command.DateOfBirth != patient.DateOfBirth) ? command.DateOfBirth : (DateTimeOffset?)null
            });
        }

        public void EnactCommand(UpdateInsurance command)
        {
            RecordEvent(new InsuranceUpdated
            {
                InsuranceCompany = command.InsuranceCompany,
                Plan = command.Plan,
                EffectiveDate = command.EffectiveDate,
                PolicyDate = command.PolicyDate,
                IssueDate = command.IssueDate
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
    }
}
