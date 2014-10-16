using System.Web.Optimization;

namespace BohFoundation.WebApi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/applicationinsights").Include("~/Scripts/ApplicationInsights.js"));

            bundles.Add(new ScriptBundle("~/bundles/scriptjs", "https://cdnjs.cloudflare.com/ajax/libs/script.js/2.5.7/script.min.js").IncludeDirectory(
                        "~/Scripts/cdn/scriptjs", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular", "https://ajax.googleapis.com/ajax/libs/angularjs/1.2.25/angular.min.js").IncludeDirectory(
                        "~/Scripts/cdn/angular", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularroutes", "https://ajax.googleapis.com/ajax/libs/angularjs/1.2.25/angular-route.min.js").IncludeDirectory(
                        "~/Scripts/cdn/angular-route", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angulartouch", "https://ajax.googleapis.com/ajax/libs/angularjs/1.2.25/angular-touch.min.js").IncludeDirectory(
                        "~/Scripts/cdn/angular-touch", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularresource", "https://ajax.googleapis.com/ajax/libs/angularjs/1.2.25/angular-resource.min.js").IncludeDirectory(
                        "~/Scripts/cdn/angular-resource", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularanimate", "https://ajax.googleapis.com/ajax/libs/angularjs/1.2.25/angular-animate.min.js").IncludeDirectory(
                        "~/Scripts/cdn/angular-animate", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/ui-bootstrap", "https://cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/0.11.0/ui-bootstrap-tpls.min.js").Include(
                        "~/Scripts/cdn/ui-bootstrap-tpls-0.11.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/minorfiles")
                .Include(
                        "~/Scripts/i18n/angular-locale_en-us.js",
                        "~/Scripts/minorfiles/angular-local-storage-8-14-2014.js",
                        "~/Scripts/minorfiles/loading-bar-0.6.0.js",
                        "~/Scripts/minorfiles/angular-toaster-0.4.7.js",
                        "~/Scripts/minorfiles/angular-ui-ui-utils-keypress-12-28-2013.js",
                        "~/Scripts/minorfiles/angular-css-injector-1.0.4.js",
                        "~/Scripts/minorfiles/textAngular-sanitize-textAngular-1.2.2.min.js",
                        "~/Scripts/minorfiles/rangy-core-1.2.3.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/angularapp")
                        .IncludeDirectory("~/AngularApp/Main", "*.js", true)
                        .IncludeDirectory("~/AngularApp/Home", "*.js", true)
                        .IncludeDirectory("~/AngularApp/UserAccount", "*.js", true)
                        .IncludeDirectory("~/AngularApp/Admin", "*.js", true)
                        .IncludeDirectory("~/AngularApp/Reference", "*.js", true)
                        .IncludeDirectory("~/AngularApp/ApplicationEvaluator", "*.js", true)
                        .IncludeDirectory("~/AngularApp/Person", "*.js", true)
                        .IncludeDirectory("~/AngularApp/Common", "*.js", true)
                        .IncludeDirectory("~/AngularApp/Dtos", "*.js", true)
                        .IncludeDirectory("~/AngularApp/Applicant", "*.js", true)
                        );

            bundles.Add(new ScriptBundle("~/bundles/dropzone", "https://cdnjs.cloudflare.com/ajax/libs/dropzone/3.10.2/dropzone.min.js").Include(
                       "~/Scripts/cdn/dropzone-3.10.2.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/site.css",
                       "~/Content/toaster-0.4.7.css",
                       "~/Content/loading-bar-0.6.0.css",
                       "~/Content/dropzone-3.10.2.css"));

            bundles.Add(new StyleBundle("~/Content/Bootstrap", "https://maxcdn.bootstrapcdn.com/bootswatch/3.2.0/yeti/bootstrap.min.css").Include(
                       "~/Content/bootstrap-yeti-3.2.0.css"));
        }
    }
}