﻿using System.Web;
using System.Web.Optimization;

namespace VidaHouseManage
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/matrix").Include(
                   "~/Scripts/jquery.matrix*"));

            bundles.Add(new ScriptBundle("~/bundles/excanvas").Include(
                        "~/Scripts/jquery.excanvas.min.js"
                        , "~/Scripts/jquery.masked.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryflot").Include(
                      "~/Scripts/jquery.flot*",                   
                      "~/Scripts/jquery.gritter.min.js",
                      "~/Scripts/jquery.peity.min.js"                      
                      ));
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-colorpicker.js",                    
                      "~/Scripts/bootstrap-wysihtml5.js",
                      "~/Scripts/respond.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-responsive.min.css",
                      "~/Content/colorpicker.css",                     
                      "~/Content/font-awesome.css",                     
                      "~/Content/jquery.gritter.css",
                      "~/Content/matrix-login.css",
                      "~/Content/matrix-media.css",
                      "~/Content/matrix-style.css",                      
                      "~/Content/uniform.css",
                      "~/Content/bootstrap-wysihtml5.css"
                      ));

            
        }
    }
}
