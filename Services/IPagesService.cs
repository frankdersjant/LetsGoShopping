using DomainModels;
using System.Collections.Generic;

namespace Services
{
    public interface IPagesService
    {
        IEnumerable<Page> GetAllPages();
        string CreatePage(Page page);
        Page GetPage(int id);
        void DeletePage(int id);
        void EditPage(Page page);
    }
}
