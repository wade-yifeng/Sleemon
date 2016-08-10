using System.Web.Optimization;

namespace Sleemon.Portal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Assets/js")
                .Include("~/Assets/components/angular/angular.min.js")
                .Include("~/Assets/components/angular-animate/angular-animate.min.js")
                .Include("~/Assets/components/angular-sanitize/angular-sanitize.min.js")
                //.Include("~/Assets/components/angular-loading-bar/loading-bar.min.js")
                .Include("~/Assets/components/angular-touch/angular-touch.min.js")
                .Include("~/Assets/components/angular-resource/angular-resource.min.js")
                .Include("~/Assets/components/oclazyload/ocLazyLoad.min.js")
                .Include("~/Assets/components/angular-ui-router/angular-ui-router.min.js")
                .Include("~/Assets/components/angular-bootstrap/ui-bootstrap-tpls.min.js")
                .Include("~/Assets/components/ngstorage/ngStorage.min.js")
                .Include("~/Assets/components/bootstrap/bootstrap.min.js")
                .Include("~/Assets/components/bootstrap/transition.min.js")
                .Include("~/Assets/js/ace-small.min.js")
                .Include("~/Assets/js/ace-elements.min.js")
                .Include("~/Assets/components/ueditor/ueditor.config.js")
                .Include("~/Assets/components/ueditor/ueditor.all.min.js")
                .Include("~/Assets/components/ueditor/lang/zh-cn/zh-cn.js"));


            //// ng逻辑代码 - 测试ace
            //bundles.Add(new ScriptBundle("~/Assets/ng")
            //    .Include("~/Assets/js/app.js")
            //    .IncludeDirectory("~/Assets/js/directives", "*.js")
            //    .IncludeDirectory("~/Assets/js/controllers", "*.js"));

            // ng逻辑代码
            bundles.Add(new ScriptBundle("~/Assets/ng")
                .Include("~/Assets/js/site.js")
                .IncludeDirectory("~/Scripts/Common", "*.js")
                .Include("~/Scripts/SleemonPortal.js")
                .IncludeDirectory("~/Scripts/Directives", "*.js")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Services", "*.js"));

            bundles.Add(new StyleBundle("~/Assets/css")
                .Include("~/Assets/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/components/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/css/ace-fonts.css", new CssRewriteUrlTransform())
                .Include("~/Assets/css/ace.min.css", new CssRewriteUrlTransform())
                //.Include("~~/Assets/components/loading-bar.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/css/ace-skins.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/css/site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}