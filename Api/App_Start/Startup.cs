using System;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu;
using AllAcu.Controllers;
using Domain;
using Domain.Authentication;
using Domain.Verification;
using Its.Configuration;
using Its.Log.Monitoring;
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
            Task.WaitAll(Configure(app));
        }

        public async Task Configure(IAppBuilder app)
        {
            container = new PocketContainer();
            httpConfiguration = new HttpConfiguration();
            RegisterStringT();

            Domain.Biller.Biller.AllAcuBillerId =
            Biller.AllAcuBillerId = BillerController.AllAcuBillerId = Settings.Get<AllAcuBiller>().BillerId;

            ConfigureWebApi(app, container, httpConfiguration);
            ConfigureCqrs(app, container);
            ConfigureContainer(app, container, httpConfiguration);
            ConfigureAuth(app, container);

            httpConfiguration.MapTestRoutes(configureTargets: targets => targets.Add("self", "self", new Uri("http://localhost:7822")));
            httpConfiguration.MapSensorRoutes(_ => true);

            app.UseWebApi(httpConfiguration);

            await BootstrapSite(app, container);
        }

        private static void RegisterStringT()
        {
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new State(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new City(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new PostalCode(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new PhoneNumber(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => new Role(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => Gender.Parse(s.ToString()));
            Serializer.AddPrimitiveConverter(s => s.ToString(), s => VerificationRequestStatus.Parse(s.ToString()));
        }
    }
}
