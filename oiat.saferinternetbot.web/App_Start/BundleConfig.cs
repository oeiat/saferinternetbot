using System.Linq;
using ClientDependency.Core;

namespace oiat.saferinternetbot.web
{
    public static class BundleConfig
    {
        private const string _baseCssPath = "~/Content/";
        private const string _baseJsPath = "~/Scripts/";

        public static void Register()
        {
            CreateCssBundle("core", 1, "bootstrap.css", "toastr.css");
            CreateJsBundle("core", 1, "jquery-3.3.1.js", "bootstrap.js", "toastr.js");
        }

        private static void CreateCssBundle(string name, int priority, params string [] fileNames)
        {
            BundleManager.CreateCssBundle(name, priority, fileNames.Select(x => new CssFile($"{_baseCssPath}{x}")).ToArray());            
        }

        private static void CreateJsBundle(string name, int priority, params string[] fileNames)
        {
            BundleManager.CreateJsBundle(name, priority, fileNames.Select(x => new JavascriptFile($"{_baseJsPath}{x}")).ToArray());
        }
    }
}