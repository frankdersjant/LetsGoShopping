using DomainModels;
using System.Collections.Generic;

namespace Services
{
    public interface IOrderDetailService
    {
        IEnumerable<OrderDetail> GetAllOrderDetails();
        void AddOrderDetail(OrderDetail orderdetail);
    }
}
