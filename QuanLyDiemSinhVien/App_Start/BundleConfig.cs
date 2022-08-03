using System.Web;
using System.Web.Optimization;

namespace QuanLyDiemSinhVien
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //Bootstrap 3.3.7
                      "~/Assets/bower_components/bootstrap/dist/css/bootstrap.min.css"
                      //Font Awesome
                      , "~/Assets/bower_components/font-awesome/css/font-awesome.min.css"
                      //Ionicons
                      , "~/Assets/bower_components/Ionicons/css/ionicons.min.css"
                      //DataTable
                      , "~/Assets/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css"
                      //Date Picker
                      , "~/Assets/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css"
                      //Theme style
                      , "~/Assets/dist/css/AdminLTE.min.css"
                      //AdminLTE Skins. Choose a skin from the css/skins folder instead of downloading all of them to reduce the load
                      , "~/Assets/dist/css/skins/_all-skins.min.css"
                      //Date Picker
                      , "~/Assets/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css"
                      , "~/Assets/bower_components/bootstrap-daterangepicker/daterangepicker.css"
                      , "~/Assets/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"
                      //Site
                      , "~/Content/Site.css"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                    "~/Assets/bower_components/jquery/dist/jquery.min.js"
                    , "~/Assets/bower_components/bootstrap/dist/js/bootstrap.min.js"
                    //datepicker
                    , "~/Assets/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"
                    //Bootstrap WYSIHTML5
                    , "~/Assets/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"
                    //DataTable
                    , "~/Assets/bower_components/datatables.net/js/jquery.dataTables.min.js"
                    , "~/Assets/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"
                    , "~/Assets/bower_components/datatables.net/js/jquery.dataTable.custom.js"
                    //DateTime Pick
                    , "~/Assets/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"
                    //Slimscroll
                    , "~/Assets/bower_components/jquery-slimscroll/jquery.slimscroll.min.js"
                    //FastClick
                    , "~/Assets/bower_components/fastclick/lib/fastclick.js"
                    //AdminLTE App
                    , "~/Assets/dist/js/adminlte.min.js"
                    //, "~/Assets/dist/js/pages/dashboard.js"
                    //, "~/Assets/dist/js/pages/demo.js"
                    ));
        }
    }
}
