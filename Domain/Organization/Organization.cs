using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.Organization
{
    public partial class Organization : EventSourcedAggregate<Organization>
    {
        public Guid? ProviderId { get; set; }

        public string BusinessRole { get; set; }
        public IList<UserAccess> Users { get; set; }
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
