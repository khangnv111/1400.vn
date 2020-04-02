using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using W1400.Utility;

namespace CMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CMS.Controllers.HomeController" }
            );
        }

        public static void RegisterRoutesCustom(RouteCollection routes)
        {
            var lst = ConfigReWriteUrl.Instance().ListReWrite;
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    routes.MapRoute(
                        name: item.routeName,
                        url: item.routeURL,
                        defaults: new { controller = item.controller, action = item.action, id = UrlParameter.Optional }
                    );
                }
            }
        }
    }
}
