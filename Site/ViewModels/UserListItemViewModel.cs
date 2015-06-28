using System;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class UserListItemViewModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }

    public class UserListItemViewModelHandler :
        IUpdateProjectionWhen<User.Registered>,
        IUpdateProjectionWhen<User.Updated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public void UpdateProjection(User.Registered @event)
        {
            dbContext.UserList.Add(new UserListItemViewModel
            {
                UserId = @event.AggregateId,
                Name = "New User"
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
