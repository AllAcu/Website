using System.Web.Mvc;
using System.Web.Routing;

namespace AllAcu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute(".htm");
            routes.IgnoreRoute(".html");

            routes.MapRoute(
                name: "claims",
                url: "claims/",
                defaults: new { controller = "Claims", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
