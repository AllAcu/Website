using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using AllAcu.Authentication;
using Domain.Biller;
using Domain.CareProvider;
using Domain.ClaimFiling;
using Domain.User;
using Its.Configuration;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Sql;
using Owin;
using Pocket;

namespace AllAcu
{
    public partial class Startup
    {
        protected IDisposable _eventSubscriptions;

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

            container.Register(typeof(IEventSourcedRepository<CareProvider>), c => Microsoft.Its.Domain.Configuration.Current.Repository<CareProvider>());
            container.Register(typeof(IEventSourcedRepository<User>), c => Microsoft.Its.Domain.Configuration.Current.Repository<User>());
            container.Register(typeof(IEventSourcedRepository<Domain.Verification.InsuranceVerification>), c => Microsoft.Its.Domain.Configuration.Current.Repository<Domain.Verification.InsuranceVerification>());
            container.Register(typeof(IEventSourcedRepository<ClaimFilingProcess>), c => Microsoft.Its.Domain.Configuration.Current.Repository<ClaimFilingProcess>());
            container.Register(typeof(IEventSourcedRepository<Biller>), c => Microsoft.Its.Domain.Configuration.Current.Repository<Biller>());

            // catch completely up
            new ReadModelCatchup<AllAcuSiteDbContext>((Discover.ProjectorTypes()
                .Select(container.Resolve).ToArray())).Run();

            var immediateSubscriptions = new[]
            {
                typeof(PatientDetailsViewModelHandler),
                typeof(InsuranceVerificationFormEventHandler),
                typeof(UserDetailsViewModelHandler),
                typeof(CareProviderInformationHandler),
                typeof(InviteHandler),
                typeof(ProviderRoleHandler),
                typeof(AllAcuBillerHandler),
                typeof(BillerRoleHandler)
            };

            _eventSubscriptions = Microsoft.Its.Domain.Configuration.Current.EventBus.Subscribe(
                immediateSubscriptions.Select(container.Resolve).ToArray());

            var catchup = new ReadModelCatchup<AllAcuSiteDbContext>((Discover.ProjectorTypes()
                .Where(s => !immediateSubscriptions.Contains(s)))
                .Select(container.Resolve).ToArray());

            catchup.Progress.Subscribe(m => Debug.WriteLine(m));
            container.RegisterSingle(c => catchup);
            catchup.PollEventStore();

            Command<CareProvider>.AuthorizeDefault = (provider, command) =>
            {
                command.Principal = CurrentPrincipal();
                return true;
            };
            Command<Biller>.AuthorizeDefault = (biller, command) =>
            {
                command.Principal = CurrentPrincipal();
                return true;
            };
            Command<Domain.Verification.InsuranceVerification>.AuthorizeDefault = (provider, command) =>
            {
                command.Principal = CurrentPrincipal();
                return true;
            };
            Command<User>.AuthorizeDefault = (provider, command) =>
            {
                if (command is User.CreateSystemUser)
                {
                    return true;
                }

                command.Principal = CurrentPrincipal();
                return true;
            };
            Command<ClaimFilingProcess>.AuthorizeDefault = (provider, command) =>
            {
                command.Principal = CurrentPrincipal();
                return true;
            };

            Microsoft.Its.Domain.Configuration.Current.UseSqlEventStore();
        }

        private static IPrincipal CurrentPrincipal()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User : new AnonymousPrincipal();
        }

        private class AnonymousPrincipal : IPrincipal, IIdentity
        {
            public string Name => "Anonymous";
            public string AuthenticationType => "Anonymous";
            public bool IsInRole(string role) => false;
            public IIdentity Identity => this;
            public bool IsAuthenticated => false;
        }
    }
}
