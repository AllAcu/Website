using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual IList<ProviderRole> ProviderRoles { get; set; } = new List<ProviderRole>();
        public virtual IList<ProviderInvitation> ProviderInvitations { get; set; } = new List<ProviderInvitation>();
        public virtual IList<BillerRole> BillerRoles { get; set; } = new List<BillerRole>();
        public virtual IList<BillerInvitation> BillerInvitations { get; set; } = new List<BillerInvitation>();
    }

    public class UserEventHandler :
        IUpdateProjectionWhen<Domain.User.User.SignedUp>,
        IUpdateProjectionWhen<Domain.User.User.Updated>,
        IUpdateProjectionWhen<Domain.User.User.BillerSystemUserInitialized>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public UserEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.User.User.SignedUp @event)
        {
            dbContext.Users.Add(new User
            {
                UserId = @event.AggregateId,
                Email = @event.Email
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.User.User.Updated @event)
        {
            var user = dbContext.Users.Find(@event.AggregateId);
            user.Name = @event.Name;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.User.User.BillerSystemUserInitialized @event)
        {
            dbContext.Users.Add(new User
            {
                UserId = @event.AggregateId,
                Name = @event.Name,
                Email = @event.Email
            });

            dbContext.SaveChanges();
        }
    }

    public static class BillerRoleList_Extensions
    {
        public static BillerRole AllAcu(this IList<BillerRole> roles)
        {
            return roles?.FirstOrDefault(r => r.Biller.Id == Domain.Biller.Biller.AllAcuBillerId);
        }
    }
}
