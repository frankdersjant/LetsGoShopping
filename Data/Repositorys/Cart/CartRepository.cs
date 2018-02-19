using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repositorys.Categories
{
    public class CartRepository : GenericRepository<Cart>,  ICartRepository
    {
        public CartRepository(ShoppingCartContext context) : base(context)
        {

        }

        public override void Add(Cart entity)
        {
            base.Add(entity);
        }

        public override void Delete(Cart entity)
        {
            base.Delete(entity);
        }

        public override void Edit(Cart entity)
        {
            base.Edit(entity);
        }

        public override IEnumerable<Cart> FindBy(Expression<Func<Cart, bool>> predicate)
        {
            return base.FindBy(predicate);
        }

        public override IEnumerable<Cart> GetAll()
        {
            return base.GetAll();
        }
    }
}
