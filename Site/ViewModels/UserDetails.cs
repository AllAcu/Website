using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Authentication;
using Domain.CareProvider;
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
        public virtual IList<Invitation> OutstandingInvites { get; set; } = new List<Invitation>();
    }

    public class UserDetailsViewModelHandler :
        IUpdateProjectionWhen<User.SignedUp>,
        IUpdateProjectionWhen<User.Updated>
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
    }
}
