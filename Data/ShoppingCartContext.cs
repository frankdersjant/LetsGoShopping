using Data.OWIN;
using DomainModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Data
{
    public class ShoppingCartContext :  IdentityDbContext<AppUser>
    {
        public DbSet<Page> Pages { get; set; }
        public DbSet<SideBar> SideBars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public ShoppingCartContext() : base("LetsGoShopping")
        {

        }

        public static ShoppingCartContext Create()
        {
            return new ShoppingCartContext();
        }
    }
}
