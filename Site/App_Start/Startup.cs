using System.Web.Http;
using AllAcu;
using Domain;
using Domain.CareProvider;
using Microsoft.Its.Domain.Serialization;
using Microsoft.Owin;
using Owin;
using Pocket;

[assembly: OwinStartup(typeof(Startup))]

namespace AllAcu
{
    public partial class Startup
    {
        private static PocketContainer container;
        internal static HttpConfiguration httpConfiguration { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            container = new PocketContainer();
            httpConfiguration = new HttpConfiguration();
            RegisterStringT();

            ConfigureMvc(app, container);
            ConfigureWebApi(app, container, httpConfiguration);
            ConfigureCqrs(app, container);
            ConfigureContainer(app, container, httpConfiguration);
            ConfigureAuth(app, container);

            app.UseWebApi(httpConfiguration);
        }

        private static void RegisterStringT()
        {
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new State(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new City(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new PostalCode(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new PhoneNumber(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => Gender.Parse(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => PendingVerification.RequestStatus.Parse(s.ToString()));
        }
    }
}
