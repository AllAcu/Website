using System;
using System.Linq;
using Domain.CareProvider.Commands;

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
                Name = command.Name,
                Gender = command.Gender,
                DateOfBirth = command.DateOfBirth,
                Address = new Address
                {
                    Line1 = command.Address1,
                    Line2 = command.Address2,
                    City = new City(command.City),
                    State = new State(command.State),
                    PostalCode = new PostalCode(command.PostalCode)
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
                UpdatedAddress = (command.Address != patient.Address) ? command.Address : null
            });
        }

        public void EnactCommand(UpdateInsurance command)
        {
            RecordEvent(new InsuranceUpdated
            {
                PatientId = command.PatientId,
                InsuranceCompany = command.InsuranceCompany,
                Plan = command.Plan,
                ProviderNumber = command.ProviderNumber,
                InsuranceId = command.InsuranceId,
                GroupNumber = command.GroupNumber,
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
