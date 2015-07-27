using System;
using System.Linq;
using Domain.Authentication;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class ProviderRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual UserDetails User { get; set; }
        public virtual CareProviderDetails Provider { get; set; }
        public RoleList Roles { get; set; } = new RoleList();
    }

    public class ProviderRoleHandler :
        IUpdateProjectionWhen<CareProvider.UserJoined>,
        IUpdateProjectionWhen<CareProvider.UserLeft>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public ProviderRoleHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.UserJoined @event)
        {
            dbContext.ProviderRoles.Add(new ProviderRole
            {
                Provider = dbContext.CareProviders.Find(@event.AggregateId),
                User = dbContext.UserDetails.Find(@event.UserId),
                Roles = new RoleList { Roles.Provider.Practitioner.ToString() }
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.UserLeft @event)
        {
            var role = dbContext.ProviderRoles.FirstOrDefault(i => i.User.UserId == @event.UserId && i.Provider.Id == @event.AggregateId);

            dbContext.ProviderRoles.Remove(role);

            dbContext.SaveChanges();
        }
    }
}
