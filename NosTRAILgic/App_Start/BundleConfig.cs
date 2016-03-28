using System.Web.Optimization;

namespace NosTRAILgic
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/NosTRAILgic.css",
                      "~/Content/ZocialCSSSocialButtons/zocial.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui-1.11.4.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                      "~/Content/themes/base/jquery.ui.core.css",
                      "~/Content/themes/base/jquery.ui.autocomplete.css",
                      "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
