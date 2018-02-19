using Data.Repositorys;
using Data.Repositorys.Sidebar;
using DomainModels;
using System.Collections.Generic;

namespace Services.Implementation
{
    public class SideBarService : ISideBarService
    {
        private readonly ISideBarRepository _SideBarRepository;
        private readonly IUnitOfWork _unitofwork;
        
        public SideBarService(ISideBarRepository SideBarRepository, IUnitOfWork unitofwork)
        {
            _SideBarRepository = SideBarRepository;
            _unitofwork = unitofwork;
        }

        public SideBar FindSideBar(int i)
        {
            return _SideBarRepository.GetById(i);
        }

        public IEnumerable<SideBar> GetAllSideBars()
        {
            return _SideBarRepository.GetAll();
        }

        public void EditSidebar(SideBar sidebar)
        {
            _SideBarRepository.Edit(sidebar);
            _unitofwork.Save();
        }

        public void DeleteSideBar(int id)
        {
            SideBar sidebar =  _SideBarRepository.GetById(id);
            _SideBarRepository.Delete(sidebar);
            _unitofwork.Save();
        }

        public string CreateSidebar(SideBar sidebar)
        {
            _SideBarRepository.Add(sidebar);
            _unitofwork.Save();

            return string.Empty;
        }
    }
}
