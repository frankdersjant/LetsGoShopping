using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repositorys.OrderDetails
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ShoppingCartContext context) : base(context)
        {

        }

        public override void Add(OrderDetail entity)
        {
            base.Add(entity);
        }

        public override void Delete(OrderDetail entity)
        {
            base.Delete(entity);
        }

        public override void Edit(OrderDetail entity)
        {
            base.Edit(entity);
        }

        public override IEnumerable<OrderDetail> FindBy(Expression<Func<OrderDetail, bool>> predicate)
        {
            return base.FindBy(predicate);
        }

        public override IEnumerable<OrderDetail> GetAll()
        {
            return base.GetAll();
        }
    }
}
