using System;
using System.Linq;
using Domain;
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
        IUpdateProjectionWhen<CareProvider.UserLeft>,
        IUpdateProjectionWhen<CareProvider.RolesGranted>,
        IUpdateProjectionWhen<CareProvider.RolesRevoked>
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
                User = dbContext.UserDetails.Find(@event.UserId)
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.UserLeft @event)
        {
            var role = dbContext.ProviderRoles.FirstOrDefault(i => i.User.UserId == @event.UserId && i.Provider.Id == @event.AggregateId);
            dbContext.ProviderRoles.Remove(role);
            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.RolesGranted @event)
        {
            var role = dbContext.ProviderRoles.Single(i => i.User.UserId == @event.UserId && i.Provider.Id == @event.AggregateId);
            @event.Roles.ForEach(r => role.Roles.Add(r.ToString()));
            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.RolesRevoked @event)
        {
            var role = dbContext.ProviderRoles.Single(i => i.User.UserId == @event.UserId && i.Provider.Id == @event.AggregateId);
            @event.Roles.ForEach(r => role.Roles.Remove(r.ToString()));
            dbContext.SaveChanges();
        }
    }
}
