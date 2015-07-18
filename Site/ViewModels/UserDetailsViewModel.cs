using System;
using System.Collections.Generic;
using Domain.Organization;
using Domain.User;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Serialization;
using Newtonsoft.Json;

namespace AllAcu
{
    public class UserDetailsViewModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ProviderIdList Providers { get; set; } = new ProviderIdList();

        public class ProviderIdList : HashSet<Guid>
        {
            public string Serialized
            {
                get { return this.ToJson(); }
                set
                {
                    foreach (var item in JsonConvert.DeserializeObject<Guid[]>(value))
                    {
                        Add(item);
                    }
                }
            }
        }
    }


    public class UserDetailsViewModelHandler :
        IUpdateProjectionWhen<User.Registered>,
        IUpdateProjectionWhen<User.Updated>,
        IUpdateProjectionWhen<Organization.UserJoined>,
        IUpdateProjectionWhen<Organization.UserLeft>
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

        public void UpdateProjection(Organization.UserJoined @event)
        {
            var user = dbContext.UserDetails.Find(@event.UserId);
            user.Providers.Add(@event.AggregateId);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Organization.UserLeft @event)
        {
            var user = dbContext.UserDetails.Find(@event.UserId);
            user.Providers.Remove(@event.AggregateId);

            dbContext.SaveChanges();
        }
    }
}
