﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual IList<CareProviderDetails> Providers { get; set; } = new List<CareProviderDetails>();
        public virtual IList<Invitation> OutstandingInvites { get; set; } = new List<Invitation>();
    }

    public class UserDetailsViewModelHandler :
        IUpdateProjectionWhen<User.SignedUp>,
        IUpdateProjectionWhen<User.Updated>,
        IUpdateProjectionWhen<CareProvider.UserJoined>,
        IUpdateProjectionWhen<CareProvider.UserLeft>
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

        public void UpdateProjection(CareProvider.UserJoined @event)
        {
            var user = dbContext.UserDetails.Find(@event.UserId);
            var provider = dbContext.CareProviders.Find(@event.AggregateId);
            user.Providers.Add(provider);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.UserLeft @event)
        {
            var user = dbContext.UserDetails.Find(@event.UserId);
            var provider = dbContext.CareProviders.Find(@event.AggregateId);
            user.Providers.Remove(provider);

            dbContext.SaveChanges();
        }
    }
}
