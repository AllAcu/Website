using System.Web.Optimization;

namespace AllAcu
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/bundles/libs")
                .Include("~/Scripts/angular.js")
                .IncludeDirectory("~/Scripts/angular-ui", "*.js")
                .IncludeDirectory("~/Scripts", "*.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .IncludeDirectory("~/Content", "*.css", true));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/App/app.js")
                .IncludeDirectory("~/Scripts/App", "*.js", true));
        }
    }
}
