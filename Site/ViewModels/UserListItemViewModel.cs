using System;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class UserListItemViewModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UserListItemViewModelHandler :
        IUpdateProjectionWhen<User.Registered>,
        IUpdateProjectionWhen<User.Updated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public UserListItemViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(User.Registered @event)
        {
            var user = dbContext.UserDetails.Find(@event.AggregateId);

            dbContext.UserList.Add(new UserListItemViewModel
            {
                UserId = @event.AggregateId,
                Email = user.Email
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(User.Updated @event)
        {
            dbContext.UserList.Find(@event.AggregateId)
                .Name = @event.Name;

            dbContext.SaveChanges();
        }
    }
}
