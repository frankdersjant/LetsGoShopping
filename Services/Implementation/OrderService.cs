using System.Collections.Generic;
using System.Linq;
using Data.Repositorys;
using Data.Repositorys.Orders;
using DomainModels;

namespace Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _OrderRepository;
        private IUnitOfWork _unitofwork { get; set; }

        public OrderService(IOrderRepository OrderRepository, IUnitOfWork unitofwork)
        {
            _OrderRepository = OrderRepository;
            _unitofwork = unitofwork;
        }

        public void Add(Order order)
        {
            _OrderRepository.Add(order);
            _unitofwork.Save();
        }
       
        public void Edit(Order order)
        {
            _OrderRepository.Edit(order);
            _unitofwork.Save();
        }

        public IEnumerable<Order> GetAllOrders()
        {
             return _OrderRepository.GetAll();
        }

        public Order getOrderPayPal(string paypalid)
        {
            return _OrderRepository.GetAll().Where(p => p.PayPalReference == paypalid).FirstOrDefault();
        }
    }
}
