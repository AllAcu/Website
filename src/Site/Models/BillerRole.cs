using System;
using System.Linq;
using AllAcu.Models;
using Domain;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class BillerRole : UserRole<Biller>
    {
        public virtual Biller Biller
        {
            get { return Securable; }
            set { Securable = value; }
        }
    }

    public class BillerRoleEventHandler :
        IUpdateProjectionWhen<Domain.Biller.Biller.UserAdded>,
        IUpdateProjectionWhen<Domain.Biller.Biller.UserRemoved>,
        IUpdateProjectionWhen<Domain.Biller.Biller.RolesGranted>,
        IUpdateProjectionWhen<Domain.Biller.Biller.RolesRevoked>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public BillerRoleEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.Biller.Biller.UserAdded @event)
        {
            dbContext.BillerRoles.Add(new BillerRole
            {
                User = dbContext.Users.Find(@event.UserId),
                Biller = dbContext.Billers.Find(@event.AggregateId)
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Biller.Biller.UserRemoved @event)
        {
            var role = dbContext.BillerRoles.FirstOrDefault(i => i.User.UserId == @event.UserId);
            dbContext.BillerRoles.Remove(role);
            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Biller.Biller.RolesGranted @event)
        {
            var role = dbContext.BillerRoles.Single(i => i.User.UserId == @event.UserId);
            @event.Roles.ForEach(r => role.Roles.Add(r.ToString()));
            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Biller.Biller.RolesRevoked @event)
        {
            var role = dbContext.BillerRoles.Single(i => i.User.UserId == @event.UserId);
            @event.Roles.ForEach(r => role.Roles.Remove(r.ToString()));
            dbContext.SaveChanges();
        }
    }
}
