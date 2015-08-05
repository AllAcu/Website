using System;
using System.Linq;
using Domain;
using Domain.Biller;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class BillerRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual UserDetails User { get; set; }
        public virtual BillerDetails Biller { get; set; }
        public RoleList Roles { get; set; } = new RoleList();
    }

    public class BillerRoleHandler :
        IUpdateProjectionWhen<Biller.UserAdded>,
        IUpdateProjectionWhen<Biller.UserRemoved>,
        IUpdateProjectionWhen<Biller.RolesGranted>,
        IUpdateProjectionWhen<Biller.RolesRevoked>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public BillerRoleHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Biller.UserAdded @event)
        {
            dbContext.BillerRoles.Add(new BillerRole
            {
                User = dbContext.UserDetails.Find(@event.UserId),
                Biller = dbContext.Billers.Find(@event.AggregateId)
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Biller.UserRemoved @event)
        {
            var role = dbContext.BillerRoles.FirstOrDefault(i => i.User.UserId == @event.UserId);
            dbContext.BillerRoles.Remove(role);
            dbContext.SaveChanges();
        }

        public void UpdateProjection(Biller.RolesGranted @event)
        {
            var role = dbContext.BillerRoles.Single(i => i.User.UserId == @event.UserId);
            @event.Roles.ForEach(r => role.Roles.Add(r.ToString()));
            dbContext.SaveChanges();
        }

        public void UpdateProjection(Biller.RolesRevoked @event)
        {
            var role = dbContext.BillerRoles.Single(i => i.User.UserId == @event.UserId);
            @event.Roles.ForEach(r => role.Roles.Remove(r.ToString()));
            dbContext.SaveChanges();
        }
    }
}
