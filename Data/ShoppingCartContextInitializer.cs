using DomainModels;
using System.Data.Entity;

namespace Data
{
    public class ShoppingCartContextInitializer : DropCreateDatabaseIfModelChanges<ShoppingCartContext> 
    {

        protected override void Seed(ShoppingCartContext context)
        {
            Page page = new Page { Title = "home", Slug = "home", hasSidebar = true, Sorting = 1 };
            context.Pages.Add(page);
            SideBar sidebar = new SideBar { Body = "BODY" };
            context.SideBars.Add(sidebar);

            Category categoryPop = new Category { Name = "Pop", Slug = "pop" };
            Category categoryTrance= new Category { Name = "Trance", Slug = "trance" };
            context.Categories.Add(categoryPop);
            context.Categories.Add(categoryTrance);

            Product productBruce = new Product { Name="Springsteen", Slug= "springsteen", Description = "springsteen", Price=20, category= categoryPop, ImageName=null,  CategoryId= 1};
            Product productIggy = new Product { Name = "Iggy", Slug = "iggy", Description = "iggy", Price = 30, category = categoryTrance, ImageName = null, CategoryId = 2 };

            context.Products.Add(productBruce);
            context.Products.Add(productIggy);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
