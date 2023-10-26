using System.Web.Optimization;

namespace AutomationSystem.Main.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // content
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",                
                "~/Content/fontawesome-all.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/layout.css",
                "~/Content/site.css",
                "~/Content/loader.css"));

            // home site content
            bundles.Add(new StyleBundle("~/HomeContent/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/fontawesome-all.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/home-layout.css",
                "~/Content/site.css",
                "~/Content/loader.css"));

            // color schemes for registration pages
            bundles.Add(new StyleBundle("~/HomeLimet/css").Include(
                "~/Content/ColorSchemes/home-limet.css"
                ));
            bundles.Add(new StyleBundle("~/HomeOcean/css").Include(
                "~/Content/ColorSchemes/home-ocean.css"
            ));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/ace")
                .Include("~/Scripts/ace/src-min-noconflict/ace.js")
                .Include("~/Scripts/ace/src-min-noconflict/mode-html.js")
                .Include("~/Scripts/ace/src-min-noconflict/theme-crimson_editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker")
                .Include("~/Scripts/moment-with-locales.js")
                .Include("~/Scripts/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/typeahead")
                .Include("~/Scripts/handlebars.js")
                .Include("~/Scripts/typeahead.bundle.js"));

            bundles.Add(new ScriptBundle("~/bundles/pagescripts")
                .Include("~/Scripts/corabeu/corabeu-control-{version}.js")
                .Include("~/Scripts/corabeu/corabeu-form-{version}.js")
                .Include("~/Scripts/automationsystem/automationsystem-component.js")
                .Include("~/Scripts/automationsystem/email-param-control.js"));
        }

    }
}
