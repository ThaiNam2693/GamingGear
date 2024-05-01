using BusinessObject;
using DataAccess.DAOs;
using DataAccess.DataAccess;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.Filter;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class CartController : Controller
    {
        #region Setting
        IOrderRepository ord_repository;
        IProductRepository pro_repository;
        IAccountRepository acc_repository;
        private readonly IHttpContextAccessor _contx;
        public CartController(IHttpContextAccessor contx)
        {
            ord_repository = new OrderRepository();
            pro_repository = new ProductRepository();
            acc_repository = new AccountRepository();
            _contx = contx;
        }
        #endregion

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Index()
        {
            try
            {


                var get = _contx.HttpContext.Session.GetString("Session");
                var cus = JsonConvert.DeserializeObject<CustomerObject>(get);

                var listC = ord_repository.GetCartsByUsername(cus.Username).ToList();

                List<ProductObject> ProductList = new List<ProductObject>();
                List<ProductImageObject> imgs = pro_repository.GetAllProductImages().ToList();
                List<Tuple<string, int, int>> imgStatus = new List<Tuple<string, int, int>>();
                foreach (var c in listC)
                {
                    string img = "";
                    int status = 0;// 0:normal | 1: out of stock | 2: Disable
                    int stock = 0;

                    var product = pro_repository.GetAllProducts().FirstOrDefault(p => p.ProId == c.ProId);
                    product.img = imgs.Where(img => img.ProId == product.ProId).Select(img => img.ProductImage1).ToList();

                    if (product != null)
                    {
                        ProductList.Add(product);
                    }

                    c.Price = product.ProPrice - ((product.DiscountPercent * product.ProPrice) / 100);
                    c.Price = Math.Round((decimal)c.Price, 2);

                    img = product.img[0];


                    //Check out of stock
                    if (product.ProQuan <= 0 || product.ProQuan < c.Quantity)
                    {
                        status = 1;
                    }

                    if (product.IsAvailable == false)
                    {
                        status = 2;
                    }

                    Tuple<string, int, int> tuple = new Tuple<string, int, int>(img, (int)product.ProQuan, status);
                    imgStatus.Add(tuple);
                }

                ViewBag.imgStatus = imgStatus;
                ViewBag.ListCart = listC;
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }
        }

        [HttpPost]
        public IActionResult DeleteCart(string us, string pro_id)
        {
            try
            {
                ord_repository.DeleteCart(us, pro_id);
                int Count = ord_repository.GetCartsByUsername(us).Count();
                _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(Count));
                return Content("Success");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }


        [HttpPost]
        public IActionResult AddToCart(string pro_id, int quantity)
        {
            try
            {
                var get = _contx.HttpContext.Session.GetString("Session");
                if (!string.IsNullOrEmpty(get))
                {

                    CustomerObject cus = JsonConvert.DeserializeObject<CustomerObject>(get);


                    var List = ord_repository.GetCartsByUsername(cus.Username).ToList();
                    CartObject Ca = List.FirstOrDefault(ca => ca.ProId == pro_id);
                    if (Ca == null)
                    {
                        var pro = pro_repository.GetAllProducts().FirstOrDefault(p => p.ProId == pro_id);
                        CartObject c = new CartObject
                        {
                            Username = cus.Username,
                            ProId = pro.ProId,
                            ProName = pro.ProName,
                            Price = pro.ProPrice,
                            Quantity = quantity
                        };
                        ord_repository.AddCart(c);
                        var count = List.Count(c => c.Username == cus.Username) + 1;
                        _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(count));

                        var rs = new
                        {
                            noti = count,
                            quan = quantity,

                        };
                        ViewBag.cartQuan = quantity;
                        return Json(rs);
                    }
                    else
                    {
                        List[List.IndexOf(Ca)].Quantity = Ca.Quantity + quantity;
                        ord_repository.CartQuantity(List[List.IndexOf(Ca)]);
                        var count = List.Count(c => c.Username == cus.Username);
                        _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(count));

                        var rs = new
                        {
                            noti = count,
                            quan = Ca.Quantity,

                        };
                        return Json(rs);
                    }
                }
                else
                {
                    var rs = new
                    {
                        noti = "fail",
                        quan = 0,

                    };
                    ViewBag.cartQuan = quantity;
                    return Json(rs);
                }
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }


        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CheckOutFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Checkout()
        {

            try
            {
                var get = _contx.HttpContext.Session.GetString("Session");
                var cus = JsonConvert.DeserializeObject<CustomerObject>(get);
                var CartList = ord_repository.GetCartsByUsername(cus.Username);


                foreach (var c in CartList)
                {
                    var product = pro_repository.GetAllProducts().FirstOrDefault(p => p.ProId == c.ProId);

                    c.Price = product.ProPrice - ((product.DiscountPercent * product.ProPrice) / 100);
                    c.Price = Math.Round((decimal)c.Price, 2);
                }
                ViewBag.ListCart = CartList;
                ViewBag.Addresses = acc_repository.GetDeliveryAddress(cus.Username);
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult Checkout(DeliveryAddressObject a, string des, double bill)
        {

            try
            {
                var get = _contx.HttpContext.Session.GetString("Session");
                var cus = JsonConvert.DeserializeObject<CustomerObject>(get);
                var CartList = ord_repository.GetCartsByUsername(cus.Username).ToList();


                foreach (var c in CartList)
                {
                    var product = pro_repository.GetAllProducts().FirstOrDefault(p => p.ProId == c.ProId);

                    c.Price = product.ProPrice - ((product.DiscountPercent * product.ProPrice) / 100);
                    c.Price = Math.Round((decimal)c.Price, 2);
                }
                int kq = ord_repository.Checkout(CartList, des, bill, a);
                int Count = ord_repository.GetCartsByUsername(cus.Username).Count();
                _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(Count));
                return Content(kq.ToString());
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult UpdateQuantity(CartObject c)
        {
            try
            {
                string alert = ord_repository.CartQuantity(c);

                var listCart = ord_repository.GetCartsByUsername(c.Username);
                var thisCart = listCart.SingleOrDefault(ca => ca.ProId == c.ProId);
                if (alert == "Out of Stock!")
                {
                    c.Quantity = thisCart.Quantity;
                }

                var quantity = c.Quantity;

                double sum = 0;

                double total_price_of_this_item = 0;

                foreach (var item in listCart)
                {
                    var product = pro_repository.GetAllProducts().FirstOrDefault(p => p.ProId == item.ProId);

                    item.Price = product.ProPrice - ((product.DiscountPercent * product.ProPrice) / 100);

                    double tmp = ((double)item.Price * (int)item.Quantity);
                    if (c.ProId == item.ProId)
                    {
                        total_price_of_this_item = tmp;
                    }

                    sum += tmp;
                }
                sum = Math.Round(sum, 2);
                total_price_of_this_item = Math.Round(total_price_of_this_item, 2);

                var rs = new
                {
                    noti = alert,
                    total = total_price_of_this_item,
                    bill = sum,
                    quanN = quantity
                };
                return Json(rs);
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult PostCheckOut()
        {
            try
            {
                var customerName = _contx.HttpContext.Session.GetString("Session");
                var customer = JsonConvert.DeserializeObject<Customer>(customerName);
                ViewBag.customer = customer.Fullname;

                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }
    }
}
