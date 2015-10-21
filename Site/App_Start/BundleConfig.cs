using System.Web.Optimization;
using System.Web.Optimization.React;

namespace AllAcu
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/libs")
                .Include("~/Scripts/moment.js")
                .Include("~/Scripts/humanize-duration.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/angular/angular.js")
                .IncludeDirectory("~/Scripts/angular", "*.js", false));

            bundles.Add(new StyleBundle("~/Content/css")
                .IncludeDirectory("~/Content", "*.css", true));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/App/app.js")
                .IncludeDirectory("~/Scripts/App", "*.js", true));

            bundles.Add(new BabelBundle("~/bundles/jsx")
                .Include("~/Scripts/App/header.jsx")
                .Include("~/Scripts/App/footer.jsx")
                .Include("~/Scripts/App/wing.jsx")
                .IncludeDirectory("~/Scripts/App", "*.jsx", false));
        }
    }
}
