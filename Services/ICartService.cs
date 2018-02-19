using DomainModels;
using System.Collections.Generic;

namespace Services
{
    public interface ICartService
    {
        IEnumerable<Cart> GetAllCarts();
    }
}
