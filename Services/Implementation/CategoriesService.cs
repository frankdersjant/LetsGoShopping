using Data.Repositorys;
using Data.Repositorys.Categories;
using DomainModels;
using System.Collections.Generic;
using System.Linq;

namespace Services.Implementation
{
    public class CategoriesService : ICategoryService
    {
        private readonly ICategoryRepository _categoryrepo;
        private IUnitOfWork _unitofwork { get; set; }

        private bool IsNameTaken(string categoryname)
        {
            IEnumerable<Category> lstcategories = _categoryrepo.GetAll();

            if (lstcategories.Any(c => c.Name == categoryname))
                return true;
            else return false;
        }

        public CategoriesService(ICategoryRepository categoryrepo, IUnitOfWork unitofwork)
        {
            _categoryrepo = categoryrepo;
            _unitofwork = unitofwork;
        }

        public string CreateCategory(Category category)
        {
            if (IsNameTaken(category.Name))
                return "Title is Taken";
            else
            {
                if (category.Name.Contains(" "))
                {
                    category.Name.Replace(" ", "-").ToLower();
                }

                category.Slug = category.Name;

                _categoryrepo.Add(category);
                _unitofwork.Save();

                return string.Empty;
            }
        }

        public void DeleteCategory(int id)
        {
            Category category = _categoryrepo.GetById(id);
            _categoryrepo.Delete(category);
            _unitofwork.Save();
        }

        public void EditCategory(Category category)
        {
            _categoryrepo.Edit(category);
            _unitofwork.Save();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryrepo.GetAll();
        }

        public Category GetCategory(int id)
        {
            Category category = _categoryrepo.GetById(id);
            return category;
        }
    }
}
