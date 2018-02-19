using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositorys.Sidebar
{
    public class SideBarRepository : GenericRepository<SideBar>, ISideBarRepository
    {

        public SideBarRepository(ShoppingCartContext context) : base(context)
        {

        }
    }
}
