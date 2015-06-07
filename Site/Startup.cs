using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Pocket;

[assembly: OwinStartup(typeof(AllAcu.Startup))]

namespace AllAcu
{
    public partial class Startup
    {
        internal static PocketContainer container;
        internal static HttpConfiguration httpConfiguration { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            container = new PocketContainer();
            httpConfiguration = new HttpConfiguration();

            ConfigureMvc(app, container);
            ConfigureWebApi(app, container, httpConfiguration);
            ConfigureCqrs(app, container);
            ConfigureContainer(app, container, httpConfiguration);
            ConfigureAuth(app, container);
        }
    }
}
