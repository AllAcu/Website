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

            httpConfig.Formatters.Remove(httpConfig.Formatters.XmlFormatter);
            httpConfig.Formatters.JsonFormatter.SerializerSettings = Serializer.CloneSettings();
            httpConfig.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            httpConfig.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
