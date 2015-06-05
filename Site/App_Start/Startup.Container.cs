using System.Web.Http;
using Owin;
using Pocket;

namespace AllAcu
{
    public partial class Startup
    {
        internal void ConfigureContainer(IAppBuilder app, PocketContainer container)
        {
            GlobalConfiguration.Configuration.ResolveDependenciesUsing(container);
        }
    }
}
