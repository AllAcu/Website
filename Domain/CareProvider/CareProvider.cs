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
        public IList<PendingVerification> PendingVerifications { get; } = new List<PendingVerification>();

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

        public class PendingVerification
        {
            public Guid DraftId { get; set; }
            public RequestStatus Status { get; set; } = RequestStatus.Draft;
            public VerificationRequest Request { get; set; }
            public string ReviewerComments { get; set; }

            public class RequestStatus : String<RequestStatus>
            {
                public RequestStatus(string value) : base(value)
                {
                    
                }
                public static RequestStatus Parse(string value)
                {
                    if (string.Compare(value, "Draft", StringComparison.OrdinalIgnoreCase) == 0)
                        return Draft;
                    if (string.Compare(value, "Submitted", StringComparison.OrdinalIgnoreCase) == 0)
                        return Submitted;
                    throw new ArgumentException($"Not a valid request status: {value}");
                }

                public static readonly RequestStatus Draft = new RequestStatus("Draft");
                public static readonly RequestStatus Submitted = new RequestStatus("Submitted");
            }
        }
    }
}
