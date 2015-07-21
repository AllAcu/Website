using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;
using WebGrease.Css.Extensions;

namespace AllAcu
{
    public class CareProviderChooserViewModel
    {
        public Guid UserId { get; set; }
        public ProviderList Providers { get; set; } = new ProviderList();

        public class ProviderList : SerialList<Provider>
        {
        }

        public class Provider
        {
            public Guid Id { get; set; }
            public string BusinessName { get; set; }
        }
    }

    public class CareProviderChooserViewModelHandler :
        IUpdateProjectionWhen<CareProvider.UserJoined>,
        IUpdateProjectionWhen<CareProvider.UserLeft>,
        IUpdateProjectionWhen<CareProvider.ProviderUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public CareProviderChooserViewModelHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(CareProvider.UserJoined @event)
        {
            var model = dbContext.ProviderChooser.Find(@event.UserId);
            if (model == null)
            {
                model = new CareProviderChooserViewModel
                {
                    UserId = @event.UserId
                };

                dbContext.ProviderChooser.Add(model);
            }

            var provider = dbContext.CareProviders.Find(@event.AggregateId);

            model.Providers.Add(new CareProviderChooserViewModel.Provider
            {
                Id = @event.AggregateId,
                BusinessName = provider.BusinessName
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.UserLeft @event)
        {
            var model = dbContext.ProviderChooser.Find(@event.UserId);

            model.Providers.RemoveWhere(p => p.Id == @event.AggregateId);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(CareProvider.ProviderUpdated @event)
        {
            var models = dbContext.ProviderChooser.Where(u => u.Providers.Any(p => p.Id == @event.AggregateId));

            models.ForEach(u => u.Providers.First(p => p.Id == @event.AggregateId).BusinessName = @event.BusinessName);

            dbContext.SaveChanges();
        }
    }
}
