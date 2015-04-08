using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Domain;
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
            ReadModelDbContext.NameOrConnectionString =
                @"Data Source=(LocalDb)\allAcu; Integrated Security=True; MultipleActiveResultSets=False; Initial Catalog=ReadModels";

            using (var db = new ClaimsReadModelDbContext())
            {
                new ReadModelDatabaseInitializer<ClaimsReadModelDbContext>().InitializeDatabase(db);
            }

            using (var eventStore = new EventStoreDbContext())
            {
                new EventStoreDatabaseInitializer<EventStoreDbContext>().InitializeDatabase(eventStore);
            }

            container.Register(typeof(IEventSourcedRepository<ClaimFilingProcess>), c => Configuration.Current.Repository<ClaimFilingProcess>());
            Configuration.Current.EventBus.Subscribe(container.Resolve<ClaimDraftWorking>());

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
