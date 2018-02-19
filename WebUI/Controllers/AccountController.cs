using Data;
using Data.OWIN;
using DomainModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Web.Mvc;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IOrderService _orderService { get; set; }
        private IOrderDetailService _orderDetailService { get; set; }
        private IProductsService _productsService { get; set; }

        public AccountController(IOrderService orderService, IOrderDetailService orderDetailService, IProductsService productsService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _productsService = productsService;
        }

        public ActionResult Index()
        {
            return this.RedirectToAction<AccountController>(c => c.Login());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginVM model = new LoginVM();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;

                AppUser user = userManager.Find(login.Email, login.Password);
                if (user != null)
                {
                    var ident = userManager.CreateIdentity(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
                    return Redirect(login.ReturnUrl ?? Url.Action("Index", "Shop"));
                }
            }
            ModelState.AddModelError("", "Invalid username or password");

            return View(login);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterVM model = new RegisterVM();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterVM model)
        {
            var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();

            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "User");
                }
            }

            return this.RedirectToAction<ShopController>(c => c.Index());
        }

        [Authorize]
        public ActionResult Logout()
        {
            //Pff!
            IOwinContext Owincontext = HttpContext.Request.GetOwinContext();
            IAuthenticationManager authenticationManager = Owincontext.Authentication;
            var authenticationType = Owincontext.Authentication.GetAuthenticationTypes();
            authenticationManager.SignOut(authenticationType.Select(o => o.AuthenticationType).ToArray());

            return this.RedirectToAction<ShopController>(c => c.Index());
        }

        [Authorize]
        public ActionResult UserNavPartial()
        {
            var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            AppUser user = userManager.FindByName(User.Identity.Name);

            UserNavPartialVM userpartial = new UserNavPartialVM
            {
                LoggedInName = user.Email
            };

            return PartialView(userpartial);
        }

        [HttpGet]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile()
        {
            var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            AppUser user = userManager.FindByName(User.Identity.Name);

            UserProfileVM userProfile = new UserProfileVM
            {
                LastName = user.LastName,
                EMail = user.Email,
                Password = user.PasswordHash
            };

            return View("UserProfile", userProfile);
        }

        [HttpPost]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile(UserProfileVM userProfileVM)
        {
            if (!ModelState.IsValid)
            {
                return View("UserProfile");
            }

            var store = new UserStore<AppUser>(new ShoppingCartContext());
            var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            AppUser user = userManager.FindByName(User.Identity.Name);

            //TODO
            user.Email = userProfileVM.EMail;
            user.PasswordHash = userProfileVM.Password;

            if (!string.IsNullOrEmpty(userProfileVM.Password))
            {
                var result = userManager.Update(user);

                if (result.Succeeded)
                {
                    store.Context.SaveChanges();
                    TempData["SM"] = "User profile changed";
                }
            }

            return RedirectToAction("UserProfile");
        }

        [HttpGet]
        public ActionResult OrdersForUser()
        {
            decimal price = 0m;
            string name = string.Empty;
            decimal total = 0m;

            List<OrdersForUserVM> lstUserOrders = new List<OrdersForUserVM>();
            List<OrderDetail> lstUserOrderDetail = new List<OrderDetail>();

            var store = new UserStore<AppUser>(new ShoppingCartContext());
            var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            AppUser user = userManager.FindByName(User.Identity.Name);

            List<Order> lstordersloggedinuser = _orderService.GetAllOrders().Where(u => u.UserId == user.Id).ToList();

            foreach (Order order in lstordersloggedinuser)
            {
                List<OrderDetail> lstOdrdedetail = _orderDetailService.GetAllOrderDetails().Where(p => p.OrderId == order.OrderID).ToList();

                Dictionary<string, int> productsAndQty = new Dictionary<string, int>();

                foreach (OrderDetail orderdetail in lstOdrdedetail)
                {
                    Product product = _productsService.GetAllProducts().Where(x => x.Id == orderdetail.ProductId).FirstOrDefault();
                    price = product.Price;
                    name = product.Name;
                    productsAndQty.Add(name, orderdetail.Quantity);
                    total = orderdetail.Quantity * price;
                }

                lstUserOrders.Add(new OrdersForUserVM
                {
                    OrderNumber = order.OrderID,
                    Total = total,
                    UserName = order.UserId,
                    CreatedAt = order.CreatedAt,
                    ProductsandQTY = productsAndQty
                });
            }

            return View(lstUserOrders);
        }
    }
}
