using DomainModels;
using System.Collections.Generic;

namespace Services
{
    public interface IProductsService
    {
        IEnumerable<Product> GetAllProducts();
        string CreateProduct(Product category);
        Product GetProduct(int id);
        void DeleteProduct(int id);
        void EditProduct(Product product);
    }
}
