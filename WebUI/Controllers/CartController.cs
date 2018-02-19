using Constants;
using Data.OWIN;
using DomainModels;
using Microsoft.AspNet.Identity.Owin;
using PayPal.Api;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        //FAT, FAT controller...
        //TODO: most of this stuff should go to the cart sevicelayer
        private readonly ICartService _cartservice;
        private readonly IProductsService _productservice;
        private readonly IOrderService _orderservice;
        private readonly IOrderDetailService _orderdetailservice;

        public CartController(ICartService cartservice, IProductsService productservice,
                              IOrderService orderservice, IOrderDetailService orderdetailserivce)
        {
            _cartservice = cartservice;
            _productservice = productservice;
            _orderservice = orderservice;
            _orderdetailservice = orderdetailserivce;
        }

        public ActionResult Index()
        {
            var cart = Session["Cart"] as List<CartVM> ?? new List<CartVM>();
            decimal total = 0m;
            if (cart.Count == 0 || Session["Cart"] == null)
            {
                ViewBag.Message = "Cart is empty";
                return View();
            }

            foreach (var item in cart)
            {
                total += item.Total;
                ViewBag.GrandTotal = total;
            }

            return View(cart);
        }

        public ActionResult CartPartial()
        {
            int quantity = 0;
            decimal price = 0m;

            CartVM cartvm = new CartVM();
            if (Session["Cart"] != null)
            {
                var list = (List<CartVM>)Session["Cart"];

                var productsales = list.OrderBy(p => p.ProductId).GroupBy(p => p.ProductId).Select(g => new
                {
                    Product = g.Key,
                    ordercount = g.Count(),
                    totalsale = g.Sum(pv => pv.Price * pv.Quantity)
                }).OrderByDescending(x => x.ordercount).ToList();


                foreach (var item in productsales)
                {
                    quantity = quantity + item.ordercount;
                    price = price + item.totalsale;

                    cartvm.Quantity = quantity;
                    cartvm.Price = price;
                }
            }
            else
            {
                cartvm.Price = 0m;
                cartvm.Quantity = 0;
            }

            return PartialView(cartvm);
        }

        public ActionResult addToCartPartial(int id)
        {
            int quant = 0;
            decimal price = 0m;

            List<CartVM> lstcartVM = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            CartVM cartvm = new CartVM();
            Product product = _productservice.GetProduct(id);
            var productincart = lstcartVM.FirstOrDefault(x => x.ProductId == id);

            //product is not yet in Cart
            if (productincart == null)
            {
                lstcartVM.Add(new CartVM
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = 1,
                    Price = product.Price,
                    ImageName = product.ImageName
                });
            }
            else
            {
                productincart.Quantity++;
            }

            foreach (CartVM item in lstcartVM)
            {
                quant += item.Quantity;
                price += item.Price * item.Quantity;

            }
            cartvm.Quantity = quant;
            cartvm.Price = price;
            Session["cart"] = lstcartVM;

            return PartialView(cartvm);
        }

        public JsonResult IncrementProduct(int productId)
        {
            List<CartVM> lstcartVM = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            CartVM cartvm = lstcartVM.FirstOrDefault(x => x.ProductId == productId);
            cartvm.Quantity++;
            var result = new { qty = cartvm.Quantity, price = cartvm.Price };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DecrementProduct(int productId)
        {
            List<CartVM> lstcartVM = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            CartVM cartvm = lstcartVM.FirstOrDefault(x => x.ProductId == productId);
            if (cartvm.Quantity > 1)
            {
                cartvm.Quantity--;
            }
            else
            {
                cartvm.Quantity = 0;
                lstcartVM.Remove(cartvm);
            }
            var result = new { qty = cartvm.Quantity, price = cartvm.Price };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void RemoveProduct(int productId)
        {
            List<CartVM> lstcartVM = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            CartVM cartvm = lstcartVM.FirstOrDefault(x => x.ProductId == productId);
            lstcartVM.Remove(cartvm);
        }

        public ActionResult PayPalPartial()
        {
            List<CartVM> lstcartVM = Session["cart"] as List<CartVM>;
            return PartialView(lstcartVM);
        }

        public async Task<ActionResult> PlaceOrder()
        {
            Payment createdPayment = null;
            List<CartVM> lstcartVM = Session["cart"] as List<CartVM>;
            DomainModels.Order order = new DomainModels.Order();

            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                order.UserId = user.Id;
                order.CreatedAt = DateTime.Now;
                _orderservice.Add(order);
                
                OrderDetail orderDetail = new OrderDetail();
                foreach (var item in lstcartVM)
                {
                    orderDetail.OrderId = order.OrderID;
                    orderDetail.UserId = order.UserId;
                    orderDetail.ProductId = item.ProductId;
                    orderDetail.Quantity = item.Quantity;
                    _orderdetailservice.AddOrderDetail(orderDetail);
                }
            }

            #region Paypal
            var apiContext = GetApiContext();

            decimal totalCost = lstcartVM.Sum(p => p.Price * p.Quantity) * 100m;

            var payment = new Payment
            {
                experience_profile_id = AppConstants.experienceprofile,
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = "Single Payment WebShop",
                            amount = new Amount
                            {
                                currency = "EUR",
                                total =  (totalCost/100.00m).ToString(),
                            },
                            item_list = CreatePaypalItemListOrders(lstcartVM)
                        }
                    },
                redirect_urls = new RedirectUrls
                {
                    return_url = Url.Action("Return", "Cart", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Cancel", "Cart", null, Request.Url.Scheme)
                }
            };

            try
            {
                createdPayment = payment.Create(apiContext);
                order.PayPalReference = createdPayment.id;
                _orderservice.Edit(order);
            }
            catch (PayPal.PaymentsException Ex)
            {
                Debug.WriteLine(Ex.ToString());
            }

            var approvalUrl = createdPayment.links.FirstOrDefault(x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));
            #endregion Paypal

            Session["cart"] = null;
            return Redirect(approvalUrl.href);
        }

        public ActionResult Return(string payerId, string paymentId)
        {
            var order = _orderservice.getOrderPayPal(paymentId);

            var apiContext = GetApiContext();

            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            var payment = new Payment()
            {
                id = paymentId
            };

            var executedPayment = payment.Execute(apiContext, paymentExecution);

            return RedirectToAction("Thankyou");
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        private APIContext GetApiContext()
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }

        private ItemList CreatePaypalItemListOrders(List<CartVM> lstcartVM)
        {
            List<Item> lstitems = new List<Item>();
            foreach (var item in lstcartVM)
            {
                Item paypalItem = new Item
                {
                    description = item.ProductName,
                    currency = "EUR",
                    quantity = item.Quantity.ToString(),
                    price = item.Price.ToString().Replace(",", ".")
                };
                lstitems.Add(paypalItem);
            }

            ItemList itemList = new ItemList()
            {
                items = lstitems
            };
            return itemList;
        }
    }
}
