using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Pocket;

[assembly: OwinStartup(typeof(AllAcu.Startup))]

namespace AllAcu
{
    public partial class Startup
    {
        internal static PocketContainer Container;

        public void Configuration(IAppBuilder app)
        {
            var container = new PocketContainer();

            ConfigureCqrs(app, container);
            ConfigureContainer(app, container);
            ConfigureAuth(app, container);
        }
    }
}
