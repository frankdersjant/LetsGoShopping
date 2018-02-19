using Data.Repositorys;
using Data.Repositorys.OrderDetails;
using DomainModels;
using System.Collections.Generic;

namespace Services.Implementation
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _OrderDetailRepository;
        private IUnitOfWork _unitofwork { get; set; }

        public OrderDetailService(IOrderDetailRepository OrderDetailRepository, IUnitOfWork unitofwork)
        {
            _OrderDetailRepository = OrderDetailRepository;
            _unitofwork = unitofwork;
        }

        public IEnumerable<OrderDetail> GetAllOrderDetails()
        {
            return _OrderDetailRepository.GetAll();
        }

        public void AddOrderDetail(OrderDetail orderdetail)
        {
            _OrderDetailRepository.Add(orderdetail);
            _unitofwork.Save();
        }
    }
}
