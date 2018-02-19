using Data.Repositorys.Categories;
using DomainModels;
using System.Collections.Generic;

namespace Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _CartRepository;

        public CartService(ICartRepository CartRepository)
        {
            _CartRepository = CartRepository;
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            return _CartRepository.GetAll();
        }
    }
}
