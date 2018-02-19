using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositorys
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShoppingCartContext _context;

        public ShoppingCartContext Context
        {
            get { return _context; }
        }

        public UnitOfWork(ShoppingCartContext context)
        {
            _context = context;
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
