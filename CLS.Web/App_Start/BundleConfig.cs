using System.Web;
using System.Web.Optimization;

namespace CLS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/JQuery/jquery-3.5.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/JQuery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/Bootstrap/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/DataTables/jquery.dataTables.min.css",
                "~/Content/DataTables/dataTables.bootstrap4.min.css",
                "~/Content/FontAwesome/all.min.css",
                "~/Content/Bootstrap/bootstrap.min.css", 
                "~/Content/site.css",
                "~/Content/Dashboard.css",
                "~/Content/ChartJS/Chart.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/application").Include(
                "~/Scripts/Application/AjaxUtils.js",
                "~/Scripts/DataTables/jquery.dataTables.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap4.min.js",
                "~/Scripts/Libraries/jquery.timeago.js",
                "~/Scripts/Libraries/tether.min.js",
                "~/Scripts/Libraries/bootbox.all.min.js",
                "~/Scripts/Libraries/bootstrap-notify.min.js",
                "~/Scripts/Libraries/moment.js",
                "~/Scripts/ChartJS/Chart.min.js",
                "~/Scripts/Application/Global.js"));
        }
    }
}
