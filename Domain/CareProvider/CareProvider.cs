using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider : EventSourcedAggregate<CareProvider>
    {
        public static readonly Guid HardCodedId = new Guid("949D90DD-8A4F-4466-B383-1A4B78468951");

        public CareProvider(Guid? id = default(Guid?)) : base(id)
        {
        }

        public CareProvider(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public IList<Patient> Patients { get; } = new List<Patient>();
        public IList<string> Practitioners { get; } = new List<string>();
        public IList<ClaimDraft> Drafts { get; } = new List<ClaimDraft>();

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
    }
}
