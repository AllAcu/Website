using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider : EventSourcedAggregate<CareProvider>
    {
        public CareProvider(Guid? id = default(Guid?)) : base(id)
        {
        }

        public CareProvider(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public CareProvider(CreateProvider command) : base(command.AggregateId)
        {
            RecordEvent(new NewProvider
            {
                BusinessName = command.BusinessName,
                PractitionerName = command.PractitionerName,
                City = command.City
            });
        }

        public IList<Patient> Patients { get; } = new List<Patient>();
        public IList<string> Practitioners { get; } = new List<string>();
        public IList<ClaimDraft> ClaimDrafts { get; } = new List<ClaimDraft>();
        public IList<VerificationRequestDraft> VerificationRequestDrafts { get; } = new List<VerificationRequestDraft>();
        public IList<OutstandingVerification> OutstandingVerifications { get; } = new List<OutstandingVerification>();

        public string BusinessName { get; set; }
        public string PractitionerName { get; set; }
        public string City { get; set; }

        public Patient GetPatient(Guid id)
        {
            return Patients.SingleOrDefault(patient => patient.Id == id);
        }

        public bool PatientExists(Guid id)
        {
            return GetPatient(id) != null;
        }

        public class VerificationRequestDraft
        {
            public Guid DraftId { get; set; }
            public VerificationRequest Request { get; set; }
        }

        public class OutstandingVerification
        {
            public Guid RequestId { get; set; }
            public VerificationRequest Request { get; set; }
        }
    }
}
