using System.Web.Http;
using Domain;
using Domain.CareProvider;
using Microsoft.Its.Domain.Serialization;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AllAcu
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            RegisterStringT();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings = Serializer.CloneSettings();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
