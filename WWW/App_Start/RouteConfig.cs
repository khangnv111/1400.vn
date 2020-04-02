using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WWW
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Tin tức chi tiết",
                url: "{rewriteurl}-post{id}",
                defaults: new { controller = "Article", action = "Detail" }
            );

            routes.MapRoute(
                name: "Chi tiết Chương trình đã thực hiện",
                url: "{rewriteurl}-pass{id}",
                defaults: new { controller = "Program", action = "DetailPass" }
            );

            routes.MapRoute(
                name: "Chi tiết Chương trình đang thực hiện",
                url: "{rewriteurl}-pen{id}",
                defaults: new { controller = "Program", action = "DetailPending" }
            );

            routes.MapRoute(
                name: "Chương trình đã thực hiện",
                url: "chuong-trinh-da-thuc-hien",
                defaults: new { controller = "Program", action = "ProgramPass" }
            );

            routes.MapRoute(
                name: "Chương trình đang thực hiện",
                url: "chuong-trinh-dang-thuc-hien",
                defaults: new { controller = "Program", action = "ProgramProcess" }
            );

            routes.MapRoute(
                name: "Góp ý",
                url: "gop-y",
                defaults: new { controller = "Feedback", action = "Index" }
            );

            routes.MapRoute(
                name: "Danh sách tin tức cha",
                url: "tin-tuc",
                defaults: new { controller = "Article", action = "ArticleAll" }
            );

            routes.MapRoute(
                name: "Video ảnh",
                url: "video-anh",
                defaults: new { controller = "Article", action = "VideoImage" }
            );

            routes.MapRoute(
                name: "Danh mục ủng hộ trực tuyến",
                url: "ung-ho-truc-tuyen",
                defaults: new { controller = "Article", action = "OnlineSupport" }
            );
            routes.MapRoute(
               name: "Danh mục",
               url: "{UrlRewrite}",
               defaults: new { controller = "Article", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
