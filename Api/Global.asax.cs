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

namespace Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var container = new PocketContainer();

            EventStoreDbContext.NameOrConnectionString = "EventStore";
            ClaimDraftRepository.NameOrConnectionString = "ClaimDrafts";
            container.Register(typeof(IEventSourcedRepository<ClaimFilingProcess>), c => Configuration.Current.Repository<ClaimFilingProcess>());
            Configuration.Current.EventBus.Subscribe(container.Resolve<ClaimDraftWorking>());

            GlobalConfiguration.Configuration
                               .ResolveDependenciesUsing(container);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            new App_Start.Domain().Configure(Configuration.Current);
        }
    }
}
