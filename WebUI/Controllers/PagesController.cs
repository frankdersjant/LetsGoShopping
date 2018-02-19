using AutoMapper;
using DomainModels;
using Microsoft.Web.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class PagesController : Controller
    {
        private IPagesService _pageservice;
        private ISideBarService _SideBarService;

        public PagesController(IPagesService pageservice, ISideBarService sidebarservice)
        {
            _pageservice = pageservice;
            _SideBarService = sidebarservice;
        }

        public ActionResult Index(string page = "")
        {
            if (page == "")
            {
                page = "home";
            }

            if (_pageservice.GetAllPages().Where(x => x.Slug.Equals(page)).Count() == 0)
            {
                this.RedirectToAction<PagesController>(c => c.Index(""));
            }
            Page foundpage = _pageservice.GetAllPages().Where(x => x.Slug == page).FirstOrDefault();

            if (!String.IsNullOrEmpty(foundpage.Slug))
            {
                ViewBag.PageTitle = foundpage.Title;
            }

            if (foundpage.hasSidebar)
            {
                ViewBag.Sidebar = "yes";
            }
            else
            {
                ViewBag.Sidebar = "no";
            }
            var result = Mapper.Map<Page, PageVM>(foundpage);
            return View(result);
        }

        public ActionResult PartialPages()
        {
            List<PageVM> lstpagevm = new List<PageVM>();
            var pages = _pageservice.GetAllPages().Where(x => x.Slug != "home").ToList();
            var result = Mapper.Map<IEnumerable<Page>, IEnumerable<PageVM>>(pages);

            return PartialView(result);
        }

        public ActionResult SideBarPartial()
        {
            SideBarVM sidebarvm = new SideBarVM();
            //default only one sidebar?!
            SideBar sidebar = _SideBarService.FindSideBar(1);
            var result = Mapper.Map<SideBar, SideBarVM>(sidebar);

            return PartialView(result);
        }
    }
}