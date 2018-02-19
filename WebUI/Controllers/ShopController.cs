using DomainModels;
using Microsoft.Web.Mvc;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICategoryService _categoryservice;
        private readonly IProductsService _productservice;

        public ShopController(ICategoryService categoryservice, IProductsService productservice)
        {
            _categoryservice = categoryservice;
            _productservice = productservice;
        }

        public ActionResult Index()
        {
            //let op name parameter bij Index - voor routing NB
            return this.RedirectToAction<PagesController>(c => c.Index(""));
        }

        public ActionResult categoryMenuPartial()
        {
            List<CategoryVM> listcategoryVM = new List<CategoryVM>();
            var listcategories = _categoryservice.GetAllCategories();
            var result = AutoMapper.Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(listcategories);

            return PartialView(result);
        }

        public ActionResult Category(string name)
        {
            List<Product> lstproduct;
            Category category = _categoryservice.GetAllCategories().Where(x => x.Slug == name).FirstOrDefault();
            lstproduct = _productservice.GetAllProducts().Where(x => x.CategoryId == category.Id).ToList();
            ViewBag.CategoryName = _productservice.GetProduct(lstproduct.First().Id).CategoryName;
            var result = AutoMapper.Mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(lstproduct);

            return View(result);
        }

        public ActionResult ProductDetails(string name)
        {
            Product product = _productservice.GetAllProducts().Where(x => x.Name.ToLower() == name).FirstOrDefault();
            var result = AutoMapper.Mapper.Map<Product, ProductVM>(product);

            return View(result);
        }
    }
}