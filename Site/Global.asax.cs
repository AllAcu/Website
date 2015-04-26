﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AllAcu.Projections;
using Domain.Authentication;
using Domain.CareProvider;
using Domain.ClaimFiling;
using Domain.Repository;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Sql;
using Pocket;

namespace AllAcu
{
    public class AllAcuWebApplication : HttpApplication
    {
        internal static PocketContainer Container;

        protected void Application_Start()
        {
            var container = new PocketContainer();
            Container = container;

            EventStoreDbContext.NameOrConnectionString = 
                @"Data Source=(LocalDb)\allAcu; Integrated Security=True; MultipleActiveResultSets=False; Initial catalog=EventStore";
            CareProviderReadModelDbContext.ConnectionString =
                @"Data Source=(LocalDb)\allAcu; Integrated Security=True; MultipleActiveResultSets=False; Initial Catalog=CareProviders";
            ClaimsProcessReadModelDbContext.ConnectionString =
                @"Data Source=(LocalDb)\allAcu; Integrated Security=True; MultipleActiveResultSets=False; Initial Catalog=ClaimsProcess";
            AllAcuSiteDbContext.ConnectionString =
                @"Data Source=(LocalDb)\allAcu; Integrated Security=True; MultipleActiveResultSets=False; Initial Catalog=AllAcuSite";

            //using (var db = new CareProviderReadModelDbContext())
            //{
            //    new ReadModelDatabaseInitializer<CareProviderReadModelDbContext>().InitializeDatabase(db);
            //}

            //using (var db = new ClaimsProcessReadModelDbContext())
            //{
            //    new ReadModelDatabaseInitializer<ClaimsProcessReadModelDbContext>().InitializeDatabase(db);
            //}

            using (var db = new AllAcuSiteDbContext())
            {
                new ReadModelDatabaseInitializer<AllAcuSiteDbContext>().InitializeDatabase(db);
            }

            using (var eventStore = new EventStoreDbContext())
            {
                new EventStoreDatabaseInitializer<EventStoreDbContext>().InitializeDatabase(eventStore);
            }

            container.Register(typeof(IEventSourcedRepository<ClaimFilingProcess>), c => Configuration.Current.Repository<ClaimFilingProcess>());
            container.Register(typeof(IEventSourcedRepository<CareProvider>), c => Configuration.Current.Repository<CareProvider>());
            //Configuration.Current.EventBus.Subscribe(container.Resolve<ClaimDraftWorking>());
            Configuration.Current.EventBus.Subscribe(container.Resolve<PatientInformationUpdateHandler>());
            Configuration.Current.EventBus.Subscribe(container.Resolve<CareProviderInformationHandler>());

            var catchup = new ReadModelCatchup<AllAcuSiteDbContext>((Discover.ProjectorTypes().Select(handlerType => container.Resolve(handlerType)).ToArray()));
            catchup.Progress.Subscribe(m => Debug.WriteLine(m));
            container.RegisterSingle(c => catchup);
//            catchup.PollEventStore();

            Command<CareProvider>.AuthorizeDefault = (provider, command) => {
                command.Principal = new UserPrincipal(name: "Brett");
                return true; };
            Command<ClaimFilingProcess>.AuthorizeDefault = (provider, command) => {
                command.Principal = new UserPrincipal(name: "Brett");
                return true;
            };

            GlobalConfiguration.Configuration.ResolveDependenciesUsing(container);
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            new App_Start.Domain().Configure(Configuration.Current);
        }
    }
}
