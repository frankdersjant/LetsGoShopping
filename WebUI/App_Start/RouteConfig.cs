using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Account", "Account/{action}/{id}", new { controller = "Account", action = "Index", id = UrlParameter.Optional }, new[] { "WebUI.Controllers" });
            routes.MapRoute("Cart", "Cart/{action}/{id}" , new { controller = "Cart", action = "Index", id= UrlParameter.Optional }, new[] { "WebUI.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name= UrlParameter.Optional }, new[] { "WebUI.Controllers" });
            routes.MapRoute("SideBarPartial", "Pages/SideBarPartial", new { controller = "Pages", action = "SideBarPartial" }, new[] { "WebUI.Controllers" });
            routes.MapRoute("PartialPages", "Pages/PartialPages", new { controller = "Pages", action = "PartialPages" }, new[] { "WebUI.Controllers" });
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "WebUI.Controllers" });
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "WebUI.Controllers" });
        }
    }
}
