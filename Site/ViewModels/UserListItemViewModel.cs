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
            dbContext.UserList.Add(new UserListItemViewModel
            {
                UserId = @event.AggregateId,
                Name = @event.Name,
                Email = @event.Email
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
