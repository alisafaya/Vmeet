using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vmeet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultVideo",
                routeTemplate: "api/{controller}/{ext}/{filename}"
            );

            routes.MapRoute(
                name: "Toplanti_link",
                url: "toplanti/index/{id}/{key}",
                defaults: new { controller = "Toplanti", action = "Index", id = -1, key = "" }
            );
            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
