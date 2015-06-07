using System.Web.Http;
using Owin;
using Pocket;

namespace AllAcu
{
    public partial class Startup
    {
        internal void ConfigureWebApi(IAppBuilder app, PocketContainer container, HttpConfiguration httpConfig)
        {
            WebApiConfig.Configure(httpConfig);
            app.UseWebApi(httpConfig);
        }
    }
}
