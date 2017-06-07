using System.Web;
using System.Web.Optimization;

namespace Inward_Tapal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidation").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/MultiselectJs").Include(
                    "~/Scripts/jquery.multi-select.js"));
            bundles.Add(new StyleBundle("~/bundles/MultiselectCss").Include(
                "~/Content/multi-select.css",
                "~/Content/multi-select.dev.css",
                "~/Content/multi-select.dist.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datepicker.js", 
                "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery-ui.js*"));
            bundles.Add(new ScriptBundle("~/bundles/jquerynumeric").Include(
                       "~/Scripts/jquery.numeric.js" ));

            bundles.Add(new StyleBundle("~/Content/DatePickercss").Include(
                      "~/Content/DatePicker/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/Autocompletecss").Include(
                     "~/Content/Autocomplete/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/DatePicker").Include(
                      "~/Scripts/DatePicker/jquery-ui.js"));
            bundles.Add(new ScriptBundle("~/bundles/AutoComplete").Include(
                     "~/Scripts/AutoComplete/jquery-ui.js"));
            bundles.Add(new ScriptBundle("~/bundles/sidebar").Include(
                     "~/Scripts/fixedSidebar.js"));
            bundles.Add(new StyleBundle("~/Content/sidebarcss").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/fixedsidebar.css"));
        }
    }
}
