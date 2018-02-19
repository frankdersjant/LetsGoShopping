using DomainModels;
using System.Collections.Generic;

namespace Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        string CreateCategory(Category category);
        Category GetCategory(int id);
        void DeleteCategory(int id);
        void EditCategory(Category category);
    }
}
