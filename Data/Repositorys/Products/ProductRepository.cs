using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repositorys.Products
{
    public class ProductRepository : GenericRepository<Product>, IProductsRepository
    {
        public ProductRepository(ShoppingCartContext context) : base(context)
        {

        }

        public override void Add(Product entity)
        {
            base.Add(entity);
        }

        public override void Delete(Product entity)
        {
            base.Delete(entity);
        }

        public override void Edit(Product entity)
        {
            base.Edit(entity);
        }

        public override IEnumerable<Product> FindBy(Expression<Func<Product, bool>> predicate)
        {
            return base.FindBy(predicate);
        }

        public override IEnumerable<Product> GetAll()
        {
            return base.GetAll();
        }
    }
}
