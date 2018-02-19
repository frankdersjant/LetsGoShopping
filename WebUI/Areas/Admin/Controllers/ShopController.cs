using AutoMapper;
using Constants;
using DomainModels;
using Microsoft.Web.Mvc;
using PagedList;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebUI.Areas.Admin.ViewModels;
using WebUI.ViewModels;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShopController : Controller
    {
        private ICategoryService _categoryservice;
        private IProductsService _productservice;
        private IImageProcessing _imageProcessing;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;

        public ShopController(ICategoryService categoryservice, IProductsService productservice,
                              IOrderService orderService, IOrderDetailService orderDetailService,
                              IImageProcessing imageProcessing)
        {
            _categoryservice = categoryservice;
            _productservice = productservice;
            _imageProcessing = imageProcessing;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }

        public ActionResult Index()
        {
            var lstCategoryVM = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(_categoryservice.GetAllCategories());

            return View(lstCategoryVM);
        }

        [HttpPost]
        public string AddNewCategory(string catname)
        {
            Category category = new Category();
            category.Name = catname;
            _categoryservice.CreateCategory(category);

            return category.Id.ToString();
        }

        public ActionResult DeleteCategory(int id)
        {
            Category category = _categoryservice.GetCategory(id);
            _categoryservice.DeleteCategory(category.Id);

            return this.RedirectToAction<ShopController>(c => c.Index());
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            ProductVM productvm = new ProductVM();
            productvm.Categories = new SelectList(_categoryservice.GetAllCategories(), "Id", "Name");

            return View(productvm);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductVM productvm, HttpPostedFileBase file)
        {
            int id;

            if (!ModelState.IsValid)
            {
                productvm.Categories = new SelectList(_categoryservice.GetAllCategories(), "Id", "Name");
                return View(productvm);
            }
            else
            {
                Product product = new Product();
                product.Name = productvm.Name;
                product.Description = productvm.Description;
                product.Price = productvm.Price;

                Category selectedcategory = _categoryservice.GetAllCategories().Where(x => x.Id == productvm.CategoryId).FirstOrDefault();
                product.category = selectedcategory;
                product.CategoryId = selectedcategory.Id;
                product.CategoryName = selectedcategory.Name;
                if (file != null)
                {
                    product.ImageName = file.FileName;
                }
                _productservice.CreateProduct(product);

                TempData["SM"] = "You have added a product";

                id = product.Id;
                // Check if a file was uploaded
                if (file != null && file.ContentLength > 0)
                {
                    _imageProcessing.GenerateDirectoriesAndSaveImages(file, id);
                }
                else
                {
                    Category _selectedcategory = _categoryservice.GetAllCategories().Where(x => x.Id == productvm.CategoryId).FirstOrDefault();
                    productvm.Categories = new SelectList(_categoryservice.GetAllCategories(), "Id", "Name");
                    productvm.category = _selectedcategory;
                    productvm.CategoryId = _selectedcategory.Id;
                    productvm.CategoryName = _selectedcategory.Name;

                    //ModelState.AddModelError("", "No image was not uploaded - something went wrong");
                    return this.RedirectToAction<ShopController>(c => c.Products(1, null));
                }
            }

            return this.RedirectToAction<ShopController>(c => c.Products(1, null));
        }

        public ActionResult Products(int? page, int? catId)
        {

            List<Product> listproducts = new List<Product>();
            var pagenumber = page ?? 1;

            //just get all
            if (catId == null || catId == 0)
            {
                listproducts = _productservice.GetAllProducts().ToList();
            }
            else
            {
                listproducts = _productservice.GetAllProducts().Where(x => x.CategoryId == catId).ToList();
            }
            ViewBag.Categories = new SelectList(_categoryservice.GetAllCategories(), "Id", "Name").ToList();
            ViewBag.selectedCat = catId.ToString();
            ViewBag.onePageOfProducts = listproducts.OrderBy(x => x.Id).ToPagedList(pagenumber, AppConstants.producstperpage);

            return View(Mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(listproducts));
        }

        [HttpGet]
        public ActionResult EditProduct(int Id)
        {
            Product product = _productservice.GetProduct(Id);

            ProductVM productvm = new ProductVM();
            productvm.Id = product.Id;
            productvm.Name = product.Name;
            productvm.Description = product.Description;
            productvm.Price = product.Price;
            productvm.ImageName = product.ImageName;
            productvm.category = product.category;
            productvm.Categories = new SelectList(_categoryservice.GetAllCategories(), "Id", "Name");

            if (product.ImageName != null)
            {
                productvm.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + Id + "/Gallery/Thumbs")).
                    Select(fn => Path.GetFileName(fn));
            }

            return View(productvm);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductVM productvm, HttpPostedFileBase file)
        {
            DirectoryInfo originalDirectory;

            Product product = _productservice.GetProduct(productvm.Id);
            productvm.Categories = new SelectList(_categoryservice.GetAllCategories(), "Id", "Name");
            productvm.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + productvm.Id + "/Gallery/Thumbs")).
               Select(fn => Path.GetFileName(fn));

            if (!ModelState.IsValid)
            {
                return View(productvm);
            }
            else
            {
                product.Name = productvm.Name;
                product.Description = productvm.Description;
                product.Price = productvm.Price;
                product.category = productvm.category;

                if (file != null)
                {
                    product.ImageName = file.FileName;
                }
                else
                {
                    product.ImageName = productvm.ImageName;
                }

                #region  fileupload
                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentType.ToLower() == "image/jpg" ||
                      file.ContentType.ToLower() == "image/jpeg" ||
                      file.ContentType.ToLower() == "image/gif" ||
                      file.ContentType.ToLower() == "image/png")
                    {
                        originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                        string pathStringProducts = Path.Combine(originalDirectory.ToString(), "Products");
                        string pathStringThumbs = Path.Combine(originalDirectory.ToString(), "Products\\" + productvm.Id.ToString() + "\\Thumbs");

                        DirectoryInfo dirProducts = new DirectoryInfo(pathStringProducts);
                        DirectoryInfo dirProductsThumbs = new DirectoryInfo(pathStringThumbs);

                        foreach (FileInfo FileProducts in dirProducts.GetFiles())
                        {
                            FileProducts.Delete();
                        }

                        foreach (FileInfo FileThumbs in dirProductsThumbs.GetFiles())
                        {
                            FileThumbs.Delete();
                        }
                    }
                }

                _productservice.EditProduct(product);

                // Set original and thumb image paths
                originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                string pathStringProductsId = Path.Combine(originalDirectory.ToString(), "Products\\" + productvm.Id.ToString());

                if (file != null)
                {
                    var path = string.Format("{0}\\{1}", pathStringProductsId, file.FileName);
                    file.SaveAs(path);


                    string pathStringThumbs2 = Path.Combine(originalDirectory.ToString(), "Products\\" + productvm.Id.ToString() + "\\Thumbs");
                    var path2 = string.Format("{0}\\{1}", pathStringThumbs2, file.FileName);

                    // Create and save thumb
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                    #endregion
                }

                TempData["SM"] = "You have edited a product";
            }

            return this.RedirectToAction<ShopController>(c => c.Products(1, null));
        }

        public ActionResult DeleteProduct(ProductVM productvm)
        {
            Product product = _productservice.GetProduct(productvm.Id);
            _productservice.DeleteProduct(productvm.Id);

            //delete files in productfolder - keep the product directory
            DirectoryInfo originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathStringProductsId = Path.Combine(originalDirectory.ToString(), "Products\\" + productvm.Id.ToString());
            DirectoryInfo dirProducts = new DirectoryInfo(pathStringProductsId);

            foreach (FileInfo FileProducts in dirProducts.GetFiles())
            {
                FileProducts.Delete();
            }

            return this.RedirectToAction<ShopController>(c => c.Products(1, null));
        }

        [HttpGet]
        public ActionResult AllOrders()
        {
            decimal price = 0m;
            string name = string.Empty;
            decimal total = 0m;

            List<OrdersForAdminVM> lstAdmOrders = new List<OrdersForAdminVM>();
            Dictionary<string, int> productsAndQty = new Dictionary<string, int>();

            foreach (Order order in _orderService.GetAllOrders().ToList())
            {
                foreach (OrderDetail orderdetail in _orderDetailService.GetAllOrderDetails().ToList())
                {
                    Product product = _productservice.GetAllProducts().Where(x => x.Id == orderdetail.ProductId).FirstOrDefault();
                    price = product.Price;
                    name = product.Name;
                    productsAndQty.Add(product.Name, orderdetail.Quantity);
                    total = orderdetail.Quantity * price;
                }

                lstAdmOrders.Add(new OrdersForAdminVM
                {
                    OrderNumber = order.OrderID,
                    Total = total,
                    UserName = order.UserId,
                    CreatedAt = order.CreatedAt,
                    ProductsandQTY = productsAndQty
                });
            }
            return View(lstAdmOrders);
        }
    }
}