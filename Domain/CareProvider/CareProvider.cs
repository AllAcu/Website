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
                City = command.City,
                NpiNumber = command.NpiNumber,
                TaxId = command.TaxId
            });

            RecordEvent(new UserJoined
            {
                UserId = command.CreatingUserId
            });
        }

        public IList<Patient> Patients { get; } = new List<Patient>();

        public IList<UserAccess> Practitioners
        {
            get { return Users.Where(u => u.Roles.Contains(ProviderRoles.Practitioner)).ToArray(); }
        }

        public IList<ClaimDraft> ClaimDrafts { get; } = new List<ClaimDraft>();
        public IList<UserAccess> Users { get; } = new List<UserAccess>();

        public string BusinessName { get; set; }
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

    public class Role : String<Role>
    {
        public Role(string role) : base(role)
        {
        }
    }

    public class UserAccess
    {
        public UserAccess(Guid userId, params Role[] roles) : this(userId, (IEnumerable<Role>)roles)
        {
        }

        public UserAccess(Guid userId, IEnumerable<Role> roles)
        {
            UserId = userId;
            Roles = new HashSet<Role>(roles) { ProviderRoles.Know };
        }

        public Guid UserId { get; }
        public HashSet<Role> Roles { get; }
    }

    public static class ProviderRoles
    {
        public static Role Know => new Role("know");
        public static Role Owner => new Role("owner");
        public static Role Practitioner => new Role("practitioner");
        public static Role OfficeAdministrator => new Role("officeAdministrator");
    }

    public static class BillerRoles
    {
        public static Role Know => new Role("know");
        public static Role Rob => new Role("rob");
        public static Role Verifier => new Role("verifier");
    }
}
