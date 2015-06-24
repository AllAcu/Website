using System.Web.Http;
using Domain;
using Domain.CareProvider;
using Microsoft.Its.Domain.Serialization;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Pocket;

namespace AllAcu
{
    public partial class Startup
    {
        internal void ConfigureWebApi(IAppBuilder app, PocketContainer container, HttpConfiguration httpConfig)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            httpConfig.SuppressDefaultHostAuthentication();
            httpConfig.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            httpConfig.MapHttpAttributeRoutes();

            httpConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            RegisterStringT();
            httpConfig.Formatters.Remove(httpConfig.Formatters.XmlFormatter);
            httpConfig.Formatters.JsonFormatter.SerializerSettings = Serializer.CloneSettings();
            httpConfig.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            httpConfig.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
