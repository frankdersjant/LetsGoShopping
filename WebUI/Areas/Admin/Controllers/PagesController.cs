using AutoMapper;
using Constants;
using DomainModels;
using Microsoft.Web.Mvc;
using Services;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.ViewModels;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        private IPagesService _pagesService;
        private ISideBarService _sidebarservice;

        public PagesController(IPagesService pagesService, ISideBarService sidebarservice)
        {
            _pagesService = pagesService;
            _sidebarservice = sidebarservice;
        }

        public ActionResult Index()
        {
            IEnumerable<Page> pages = _pagesService.GetAllPages();
            var result = Mapper.Map<IEnumerable<Page>, IEnumerable<PageVM>>(pages);

            return View(result);
        }

        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPage(PageVM pagevm)
        {
            if (!ModelState.IsValid)
            {
                return View(pagevm);
            }

            Page page = new Page();
            page.Title = pagevm.Title;
            page.Slug = pagevm.Slug;
            page.Body = pagevm.Body;
            page.hasSidebar = pagevm.hasSidebar;
            page.Sorting = AppConstants.SlugMax;
            
            var result = _pagesService.CreatePage(page);
            if (result != string.Empty)
            {
                ModelState.AddModelError("TitleExists", "Page or title already exists");
                return View(pagevm);
            }
            else
            {
                TempData["SM"] = "Added page";
                return this.RedirectToAction<PagesController>(x => x.Index());
            }
        }

        [HttpGet]
        public ActionResult EditPage(int id)
        {
            var pagefound = _pagesService.GetPage(id);
            var result = Mapper.Map<Page, PageVM>(pagefound);

            return View(result);
        }

        [HttpPost]
        public ActionResult EditPage(PageVM pagevm)
        {
            if (!ModelState.IsValid)
            {
                return View(pagevm);
            }

            string slug = AppConstants.SlugHome;
            if (pagevm.Slug != "home")
            {
                var pagefound = _pagesService.GetPage(pagevm.Id);
                pagefound.Title = pagevm.Title;
                pagefound.Body = pagevm.Body;
                pagefound.Slug = slug;
                pagefound.hasSidebar = pagevm.hasSidebar;
                _pagesService.EditPage(pagefound);

                TempData["SM"] = "You have edited the page";
                return this.RedirectToAction<PagesController>(c => c.Index());
            }
            else
            {
                ModelState.AddModelError("Generic", "Something went wrong");
                return View(pagevm);
            }
        }

        [HttpGet]
        public ActionResult PageDetails(int Id)
        {
            var pagefound = _pagesService.GetPage(Id);
            var result = Mapper.Map<Page, PageVM>(pagefound);

            return View(result);
        }

        [HttpGet]
        public ActionResult DeletePage(int Id)
        {
            var pagefound = _pagesService.GetPage(Id);
            _pagesService.DeletePage(pagefound.Id);

            return this.RedirectToAction<PagesController>(c => c.Index());;
        }

        [HttpGet]
        public ActionResult Sidebars()
        {
            var sidebars = _sidebarservice.GetAllSideBars();
            var result = Mapper.Map<IEnumerable<SideBar>, IEnumerable<SideBarVM>>(sidebars);
           
            return View(result);
        }

        [HttpGet]
        public ActionResult CreateSidebar()
        {
            SideBarVM newsidebarvm = new SideBarVM();

            return View(newsidebarvm);
        }


        [HttpPost]
        public ActionResult CreateSidebar(SideBarVM sideBarvm)
        {
            SideBar newsidebar = new SideBar();
            newsidebar.Body = sideBarvm.Body;
            _sidebarservice.CreateSidebar(newsidebar);

            TempData["SM"] = "Created Sidebar";
            return this.RedirectToAction<PagesController>(c => c.Sidebars()); 
        }

        [HttpGet]
        public ActionResult EditSideBar(int id)
        {
            var sidebars = _sidebarservice.FindSideBar(id);
            var result = Mapper.Map<SideBar, SideBarVM>(sidebars);

            return View(result);
        }

        [HttpPost]
        public ActionResult EditSideBar(SideBarVM sidebarvm)
        {
            SideBar newsidebar = new SideBar();
            newsidebar.Id = sidebarvm.Id;
            newsidebar.Body = sidebarvm.Body;

            _sidebarservice.EditSidebar(newsidebar);

            TempData["SM"] = "Edited Sidebar";
            return this.RedirectToAction<PagesController>(c => c.Sidebars());
        }

        [HttpGet]
        public ActionResult DeleteSideBar(int id)
        {
            _sidebarservice.DeleteSideBar(id);

            TempData["SM"] = "Deleted Sidebar";
            return this.RedirectToAction<PagesController>(c => c.Sidebars());
        }
    }
}
