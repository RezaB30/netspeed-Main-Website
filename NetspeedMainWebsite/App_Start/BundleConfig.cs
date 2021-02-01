using System.Web;
using System.Web.Optimization;

namespace NetspeedMainWebsite
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery-ui-1.12.1.js",
                         "~/Scripts/inputmask/jquery.inputmask.js",
                         "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/helpbubscript.js"
                        ));


            //bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            //            "~/Scripts/inputmask/jquery.inputmask.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/animate.min.css",
                      "~/Content/login-6.css",
                      "~/Content/plugins.bundle.css",
                      "~/Content/prismjs.bundle.css",
                      "~/Content/style.bundle.css",
                      "~/Content/wizard5-basvuru.css"));
        }
    }
}
