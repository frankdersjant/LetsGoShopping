using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repositorys.Categories
{
    public class CategoryRepository : GenericRepository<Category>,  ICategoryRepository
    {
        public CategoryRepository(ShoppingCartContext context) : base(context)
        {

        }

        public override void Add(Category entity)
        {
            base.Add(entity);
        }

        public override void Delete(Category entity)
        {
            base.Delete(entity);
        }

        public override void Edit(Category entity)
        {
            base.Edit(entity);
        }

        public override IEnumerable<Category> FindBy(Expression<Func<Category, bool>> predicate)
        {
            return base.FindBy(predicate);
        }

        public override IEnumerable<Category> GetAll()
        {
            return base.GetAll();
        }
    }
}
