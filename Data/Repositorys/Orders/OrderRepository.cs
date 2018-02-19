using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repositorys.Orders
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShoppingCartContext context) : base(context)
        {

        }

        public override void Add(Order entity)
        {
            base.Add(entity);
        }

        public override void Delete(Order entity)
        {
            base.Delete(entity);
        }

        public override void Edit(Order entity)
        {
            base.Edit(entity);
        }

        public override IEnumerable<Order> FindBy(Expression<Func<Order, bool>> predicate)
        {
            return base.FindBy(predicate);
        }

        public override IEnumerable<Order> GetAll()
        {
            return base.GetAll();
        }
    }
}
