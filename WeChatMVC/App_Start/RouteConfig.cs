using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WeChatMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Upload/{*filename}");
            routes.MapRoute(
                name: "WeChat",
                url: "wechat/{action}/{id}",
                defaults: new { controller = "wechat", action = "index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Binding",
                url: "Binding/{action}",
                defaults: new { controller = "binding", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "useAPI",
                url: "api/{action}/{studentnum}/{pwd}",
                defaults: new { controller = "api", id = UrlParameter.Optional },
                constraints: new { studentnum = @"\d*" }
                );

            routes.MapRoute(
                name: "FileUpLoad",
                url: "fileupload/{action}",
                defaults: new { controller = "fileupload", action = "index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "BaiduMap", action = "index", id = UrlParameter.Optional }
            );

        }
    }
}
