using DomainModels;
using System.Collections.Generic;

namespace Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders(); 
        void Add(Order order);
        Order getOrderPayPal(string paypalid);
        void Edit(Order order);
    }
}
