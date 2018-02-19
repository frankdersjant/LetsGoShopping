using Services;
using Services.Implementation;
using StructureMap.Configuration.DSL;

namespace WebUI.Infrastructure.StructureMapRegistry
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry()
        {
            For<IPagesService>().Use<PagesService>();
            For<ICategoryService>().Use<CategoriesService>();
            For<IProductsService>().Use<ProductsService>();
            For<IPagesService>().Use<PagesService>();
            For<ISideBarService>().Use<SideBarService>();
            For<ICartService>().Use<CartService>();
            For<IOrderService>().Use<OrderService>();
            For<IOrderDetailService>().Use<OrderDetailService>();
            For<IImageProcessing>().Use<ImageProcessing>();
        }
    }
}