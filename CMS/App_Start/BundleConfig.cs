using System.Web;
using System.Web.Optimization;

namespace CMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/treeview").Include(
                        "~/Scripts/plugins/treeview/bootstrap-treeview.js",
                        "~/Scripts/plugins/treeview/jquery.treegrid.js",
                        "~/Scripts/plugins/treeview/jquery.nestable.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/hightchart").Include(
                       "~/Scripts/plugins/Hightchart/highcharts.js",
                       "~/Scripts/plugins/Hightchart/exporting.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/tableExport").Include(
                       "~/Scripts/plugins/dataTables/jquery.dataTables.js",
                       "~/Scripts/plugins/dataTables/dataTables.bootstrap.js",
                        "~/Scripts/plugins/dataTables/jszip.min.js",
                        "~/Scripts/plugins/dataTables/pdfmake.min.js",
                        "~/Scripts/plugins/dataTables/vfs_fonts.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/html5pdf").Include(

                    "~/Scripts/plugins/dataTables/html5-button/dataTables.buttons.min.js",
                    "~/Scripts/plugins/dataTables/html5-button/buttons.html5.min.js",
                    "~/Scripts/plugins/dataTables/html5-button/buttons.colVis.min.js",
                    "~/Scripts/plugins/dataTables/html5-button/buttons.print.min.js",
                    "~/Scripts/cmsFunction.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                      "~/Scripts/jquery.metisMenu.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/site.css",
                      "~/Content/sb-admin.css",
                      "~/Content/messi.css",
                      "~/Content/morris-0.4.3.min.css",
                       "~/Content/font-awesome/css/font-awesome.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/plugincss").Include(
                        "~/Scripts/dropzone/css/*.css",
                        "~/ckEditor/.css",
                        "~/Content/plugins/validation/bootstrapValidator.css",
                        "~/Content/plugins/datetimepicker/bootstrap-datetimepicker.css",
                        "~/Content/plugins/dropdown-hover/animate.min.css",
                        "~/Content/plugins/dropdown-hover/bootstrap-dropdownhover.min.css",
                        "~/Content/plugins/bootstrap-multiselect.css",
                        "~/Content/plugins/treeview/bootstrap-treeview.css",
                        "~/Content/plugins/treeview/jquery.treegrid.css",
                        "~/Content/plugins/dataTables/dataTables.bootstrap.min.css",
                        "~/Content/plugins/dataTables/buttons.dataTables.min.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                         "~/Scripts/bootstrap.min.js",
                         "~/Scripts/bootbox.min.js",
                         "~/Scripts/jquery.pager.js",
                         "~/Scripts/common.js",
                         "~/Scripts/utils.js",
                         "~/Scripts/plugins/validation/bootstrapValidator.js",
                         "~/Scripts/plugins/datetimepicker/moment.js",
                         "~/Scripts/plugins/datetimepicker/bootstrap-datetimepicker.js",
                         "~/Scripts/plugins/bootstrap-multiselect.js",
                         "~/Scripts/sb-admin.js",
                         "~/Scripts/dropzone/*.js"
                         //"~/ckEditor/*.js"
                ));

        }
    }
}
