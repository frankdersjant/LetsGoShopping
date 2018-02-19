using Data.Repositorys;
using Data.Repositorys.Pages;
using DomainModels;
using System.Collections.Generic;
using System.Linq;

namespace Services.Implementation
{
    public class PagesService : IPagesService
    {
        private readonly IPageRepository _pagerepo;
        private IUnitOfWork _unitofwork { get; set; }

        public PagesService(IPageRepository pagerepo, IUnitOfWork unitofwork)
        {
            _pagerepo = pagerepo;
            _unitofwork = unitofwork;
        }

        private bool IsNameTaken(string pagename)
        {
            IEnumerable<Page> lstpages = _pagerepo.GetAll();

            if (lstpages.Any(c => c.Title == pagename))
                return true;
            else return false;
        }

        public string CreatePage(Page page)
        {
            if (IsNameTaken(page.Title))
                return "Page already exists";
            else
            {
                if (page.Title.Contains(" "))
                {
                    page.Title.Replace(" ", "-").ToLower();
                }

                page.Slug = page.Slug.Replace(" ", "-").ToLower();
            }
            _pagerepo.Add(page);
            _unitofwork.Save();
            return string.Empty;
        }

        public void DeletePage(int id)
        {
            Page page = _pagerepo.GetById(id);
            _pagerepo.Delete(page);
            _unitofwork.Save();
        }

        public void EditPage(Page page)
        {
            _pagerepo.Edit(page);
            _unitofwork.Save();
        }

        public IEnumerable<Page> GetAllPages()
        {
            return _pagerepo.GetAll();
        }

        public Page GetPage(int id)
        {
            return _pagerepo.GetById(id);
        }
    }
}
