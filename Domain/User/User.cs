using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User : EventSourcedAggregate<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        protected IList<ProviderAccess> Providers { get; } = new List<ProviderAccess>();

        public User(Guid? id = default(Guid?)) : base(id)
        {
        }

        public User(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public User(Register command) : base(command.AggregateId != Guid.Empty ? command.AggregateId : Guid.NewGuid())
        {
            RecordEvent(new Registered
            {
                Name = command.Name,
                Email = command.Email
            });
        }

        public bool HasPermission(Guid providerId, Role role = null)
        {
            return Providers.Any(p => p.ProviderId == providerId && p.Roles.Contains(role ?? ProviderRoles.Know));
        }

        public void AddProviderAccess(Guid providerId)
        {
            Providers.Add(new ProviderAccess(providerId));
        }

        public void RemoveProviderAccess(Guid providerId)
        {
            Providers.Remove(Providers.First(p => p.ProviderId == providerId));
        }
    }

    public class ProviderAccess
    {
        public ProviderAccess(Guid providerId, params Role[] roles) : this(providerId, (IEnumerable<Role>)roles)
        {
        }

        public ProviderAccess(Guid providerId, IEnumerable<Role> roles)
        {
            ProviderId = providerId;
            Roles = roles.ToList();
            if (!Roles.Contains(ProviderRoles.Know))
            {
                Roles.Add(ProviderRoles.Know);
            }
        }

        public Guid ProviderId { get; }
        public IList<Role> Roles { get; }
    }

    public class Role : String<Role>
    {
        public Role(string role) : base(role)
        {
        }
    }

    public static class ProviderRoles
    {
        public static Role Know => new Role("know");
    }
}
