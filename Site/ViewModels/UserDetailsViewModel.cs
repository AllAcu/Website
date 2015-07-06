using System;
using System.Collections.ObjectModel;
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

        public class ProviderIdList : Collection<Guid>
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
        IUpdateProjectionWhen<User.JoinedProvider>,
        IUpdateProjectionWhen<User.LeftProvider>
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

        public void UpdateProjection(User.JoinedProvider @event)
        {
            var user = dbContext.UserDetails.Find(@event.AggregateId);
            user.Providers.Add(@event.ProviderId);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(User.LeftProvider @event)
        {
            var user = dbContext.UserDetails.Find(@event.AggregateId);
            user.Providers.Remove(@event.ProviderId);

            dbContext.SaveChanges();
        }
    }
}
