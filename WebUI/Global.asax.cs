using AutoMapper;
using Data;
using DomainModels;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebUI.ViewModels;

namespace WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<ShoppingCartContext>(new ShoppingCartContextInitializer());

            using (var context = new ShoppingCartContext())
            {
                context.Database.Initialize(force: true);
            }

            Mapper.Initialize(cfg => {
                cfg.CreateMap<Page, PageVM>();
                cfg.CreateMap<Category, CategoryVM>();
                cfg.CreateMap<SideBar, SideBarVM>();
                cfg.CreateMap<Product, ProductVM>();
            });
        }
    }
}
