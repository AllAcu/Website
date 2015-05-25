﻿using System;
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
                City = command.City,
                NpiNumber = command.NpiNumber,
                TaxId = command.TaxId
            });
        }

        public IList<Patient> Patients { get; } = new List<Patient>();
        public IList<string> Practitioners { get; } = new List<string>();
        public IList<ClaimDraft> ClaimDrafts { get; } = new List<ClaimDraft>();
        public IList<PendingVerification> PendingVerifications { get; } = new List<PendingVerification>();

        public string BusinessName { get; set; }
        public string PractitionerName { get; set; }
        public string City { get; set; }
        public string NpiNumber { get; set; }
        public string TaxId { get; set; }

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
