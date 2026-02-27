using System.Web;
using System.Web.Optimization;

namespace Labo1
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
            "~/Scripts/validation.js",
            "~/Scripts/jquery-maskedinput.js",
            "~/Scripts/bootbox.js",
            "~/Scripts/autoRefreshPanel.js",
            "~/Scripts/image-control.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
            "~/Content/_layout.css",
            "~/Content/site.css",
            "~/Content/menu.css",
            "~/Content/image-control.css",
            "~/Content/jqui-custom-datepicker.css",
            "~/Content/media.css"));
        }
    }
}
