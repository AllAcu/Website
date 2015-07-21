using System;
using Domain.CareProvider;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class UserDetailsViewModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ProviderIdList Providers { get; set; } = new ProviderIdList();

        public class ProviderIdList : SerialList<Guid>
        { }
    }

    public class UserDetailsViewModelHandler :
        IUpdateProjectionWhen<User.Registered>,
        IUpdateProjectionWhen<User.Updated>,
        IUpdateProjectionWhen<CareProvider.UserJoined>,
        IUpdateProjectionWhen<CareProvider.UserLeft>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public UserDetailsViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(User.Registered @event)
        {
            dbContext.UserDetails.Add(new UserDetailsViewModel
            {
                UserId = @event.AggregateId,
                Name = @event.Name,
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

        public void UpdateProjection(CareProvider.UserJoined @event)
        {
            var user = dbContext.UserDetails.Find(@event.UserId);
            user.Providers.Add(@event.AggregateId);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.UserLeft @event)
        {
            var user = dbContext.UserDetails.Find(@event.UserId);
            user.Providers.Remove(@event.AggregateId);

            dbContext.SaveChanges();
        }
    }
}
