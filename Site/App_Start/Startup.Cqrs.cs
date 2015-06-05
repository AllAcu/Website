using System;
using System.Diagnostics;
using System.Linq;
using AllAcu.Models;
using Domain.Authentication;
using Domain.CareProvider;
using Domain.ClaimFiling;
using Domain.Verification;
using Its.Configuration;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Sql;
using Owin;
using Pocket;

namespace AllAcu
{
    public partial class Startup
    {
        internal void ConfigureCqrs(IAppBuilder app, PocketContainer container)
        {
            var dbConnections = Settings.Get<DatabaseConnections>();
            EventStoreDbContext.NameOrConnectionString = dbConnections.EventStore;
            AllAcuSiteDbContext.ConnectionString = dbConnections.AllAcu;
            AuthorizationDbContext.ConnectionString = dbConnections.Authentication;

            using (var db = new AllAcuSiteDbContext())
            {
                new ReadModelDatabaseInitializer<AllAcuSiteDbContext>().InitializeDatabase(db);
            }

            using (var eventStore = new EventStoreDbContext())
            {
                new EventStoreDatabaseInitializer<EventStoreDbContext>().InitializeDatabase(eventStore);
            }

            container.Register(typeof(IEventSourcedRepository<ClaimFilingProcess>), c => Microsoft.Its.Domain.Configuration.Current.Repository<ClaimFilingProcess>());
            container.Register(typeof(IEventSourcedRepository<CareProvider>), c => Microsoft.Its.Domain.Configuration.Current.Repository<CareProvider>());
            container.Register(typeof(IEventSourcedRepository<InsuranceVerification>), c => Microsoft.Its.Domain.Configuration.Current.Repository<InsuranceVerification>());

            var catchup = new ReadModelCatchup<AllAcuSiteDbContext>((Discover.ProjectorTypes().Select(handlerType => container.Resolve(handlerType)).ToArray()));
            catchup.Progress.Subscribe(m => Debug.WriteLine(m));
            container.RegisterSingle(c => catchup);
            catchup.PollEventStore();

            Command<CareProvider>.AuthorizeDefault = (provider, command) => {
                command.Principal = new UserPrincipal(name: "Brett");
                return true;
            };
            Command<InsuranceVerification>.AuthorizeDefault = (provider, command) => {
                command.Principal = new UserPrincipal(name: "Brett");
                return true;
            };
            Command<ClaimFilingProcess>.AuthorizeDefault = (provider, command) => {
                command.Principal = new UserPrincipal(name: "Brett");
                return true;
            };
        }
    }
}
