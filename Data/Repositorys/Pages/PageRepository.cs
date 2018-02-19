using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repositorys.Pages
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        public PageRepository(ShoppingCartContext context) : base(context)
        {

        }

        public override void Add(Page entity)
        {
            base.Add(entity);
        }

        public override void Delete(Page entity)
        {
            base.Delete(entity);
        }

        public override void Edit(Page entity)
        {
            base.Edit(entity);
        }

        public override IEnumerable<Page> FindBy(Expression<Func<Page, bool>> predicate)
        {
            return base.FindBy(predicate);
        }

        public override IEnumerable<Page> GetAll()
        {
            return base.GetAll();
        }
    }
}
