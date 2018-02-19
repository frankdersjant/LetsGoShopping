using Data.Repositorys;
using Data.Repositorys.Categories;
using Data.Repositorys.OrderDetails;
using Data.Repositorys.Orders;
using Data.Repositorys.Pages;
using Data.Repositorys.Products;
using Data.Repositorys.Sidebar;
using StructureMap.Configuration.DSL;

namespace WebUI.Infrastructure.StructureMapRegistry
{
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<IUnitOfWork>().Use<UnitOfWork>();
            For<IPageRepository>().Use<PageRepository>();
            For<ICategoryRepository>().Use<CategoryRepository>();
            For<IProductsRepository>().Use<ProductRepository>();
            For<ISideBarRepository>().Use<SideBarRepository>();
            For<ICartRepository>().Use<CartRepository>();
            For<IOrderRepository>().Use<OrderRepository>();
            For<IOrderDetailRepository>().Use<OrderDetailRepository>();
        }
    }
}