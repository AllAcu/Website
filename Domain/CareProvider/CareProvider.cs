using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Authentication;
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
                City = command.City,
                NpiNumber = command.NpiNumber,
                TaxId = command.TaxId
            });

            RecordEvent(new UserJoined
            {
                UserId = command.CreatingUserId
            });

            RecordEvent(new RolesGranted
            {
                UserId = command.CreatingUserId,
                Roles = new[] { Roles.Owner }
            });
        }

        public IList<Patient> Patients { get; } = new List<Patient>();

        public IList<UserAccess> Practitioners
        {
            get { return Users.Where(u => u.Roles.Contains(Roles.Practitioner)).ToArray(); }
        }

        public IList<ClaimDraft> ClaimDrafts { get; } = new List<ClaimDraft>();
        public IList<UserAccess> Users { get; } = new List<UserAccess>();

        public string BusinessName { get; set; }
        public string City { get; set; }
        public string NpiNumber { get; set; }
        public string TaxId { get; set; }

        public UserAccess GetUser(Guid id)
        {
            return Users.SingleOrDefault(user => user.UserId == id);
        }

        public Patient GetPatient(Guid id)
        {
            return Patients.SingleOrDefault(patient => patient.Id == id);
        }

        public bool PatientExists(Guid id)
        {
            return GetPatient(id) != null;
        }

        public static class Roles
        {
            public static Role Owner => new Role("owner");
            public static Role Practitioner => new Role("practitioner");
            public static Role OfficeAdministrator => new Role("officeAdministrator");
        }
    }
}
