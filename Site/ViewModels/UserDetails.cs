using System;
using System.Collections.Generic;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class UserDetails
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual IList<ProviderRole> ProviderRoles { get; set; } = new List<ProviderRole>();
        public virtual IList<ProviderInvitation> ProviderInvitations { get; set; } = new List<ProviderInvitation>();
        public virtual IList<BillerInvitation> BillerInvitations { get; set; } = new List<BillerInvitation>();
    }

    public class UserDetailsViewModelHandler :
        IUpdateProjectionWhen<User.SignedUp>,
        IUpdateProjectionWhen<User.Updated>,
        IUpdateProjectionWhen<User.BillerSystemUserInitialized>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public UserDetailsViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(User.SignedUp @event)
        {
            dbContext.UserDetails.Add(new UserDetails
            {
                UserId = @event.AggregateId,
                Email = @event.Email
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(User.Updated @event)
        {
            var user = dbContext.UserDetails.Find(@event.AggregateId);
            user.Name = @event.Name;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(User.BillerSystemUserInitialized @event)
        {
            dbContext.UserDetails.Add(new UserDetails
            {
                UserId = @event.AggregateId,
                Name = @event.Name,
                Email = @event.Email
            });

            dbContext.SaveChanges();
        }
    }
}
