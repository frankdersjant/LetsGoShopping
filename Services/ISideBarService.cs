using DomainModels;
using System.Collections.Generic;

namespace Services
{
    public interface ISideBarService
    {
        SideBar FindSideBar(int i);
        IEnumerable<SideBar> GetAllSideBars();
        string CreateSidebar(SideBar sidebar);
        void EditSidebar(SideBar sidebar);
        void DeleteSideBar(int id);

    }
}
