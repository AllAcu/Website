using System;
using System.Linq;
using Domain;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class ProviderRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual User User { get; set; }
        public virtual CareProvider Provider { get; set; }
        public RoleList Roles { get; set; } = new RoleList();
    }

    public class ProviderRoleEventHandler :
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.UserJoined>,
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.UserLeft>,
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.RolesGranted>,
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.RolesRevoked>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public ProviderRoleEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.CareProvider.CareProvider.UserJoined @event)
        {
            dbContext.ProviderRoles.Add(new ProviderRole
            {
                Provider = dbContext.CareProviders.Find(@event.AggregateId),
                User = dbContext.Users.Find(@event.UserId)
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.CareProvider.CareProvider.UserLeft @event)
        {
            var role = dbContext.ProviderRoles.FirstOrDefault(i => i.User.UserId == @event.UserId && i.Provider.Id == @event.AggregateId);
            dbContext.ProviderRoles.Remove(role);
            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.CareProvider.CareProvider.RolesGranted @event)
        {
            var role = dbContext.ProviderRoles.Single(i => i.User.UserId == @event.UserId && i.Provider.Id == @event.AggregateId);
            @event.Roles.ForEach(r => role.Roles.Add(r.ToString()));
            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.CareProvider.CareProvider.RolesRevoked @event)
        {
            var role = dbContext.ProviderRoles.Single(i => i.User.UserId == @event.UserId && i.Provider.Id == @event.AggregateId);
            @event.Roles.ForEach(r => role.Roles.Remove(r.ToString()));
            dbContext.SaveChanges();
        }
    }
}
