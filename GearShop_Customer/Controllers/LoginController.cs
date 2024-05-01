using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class LoginController : Controller
    {
        #region Setting
        IAccountRepository acc_repository;
        IOrderRepository ord_repository;
        private readonly IHttpContextAccessor _contx;
        public LoginController(IHttpContextAccessor contx)
        {
            acc_repository = new AccountRepository();
            ord_repository = new OrderRepository();
            _contx = contx;
        }
        #endregion



        [HttpGet("/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpGet("/ForgetPassword")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(string username, string password, string isRem)
        {
            try
            {
                CustomerObject cus = acc_repository.GetCustomer(username, password);

                if (cus != null)
                {
                    if (isRem != null)
                    {
                        HttpContext.Response.Cookies.Append("customer", cus.Username, new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(3),
                        });
                        HttpContext.Response.Cookies.Append("role", "0", new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(3),
                        });
                    }
                    List<DeliveryAddressObject> list = acc_repository.GetDeliveryAddress(username);
                    if (list.Count() > 0)
                    {
                        cus.addresses = new List<DeliveryAddressObject>();
                        cus.addresses.AddRange(list);
                    }

                    var count = ord_repository.GetCartsByUsername(username).Count();
                    _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(cus)); //Store information customer to Session
                    _contx.HttpContext.Session.SetString("action", JsonConvert.SerializeObject("0")); //Store action filter about manager is 0
                    _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(count));
                    return RedirectToAction("Index", "Home");
                }
                _contx.HttpContext.Session.SetString("ErrorLogin", JsonConvert.SerializeObject("Username or password is incorrect"));
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpGet("/Logout")]
        public ActionResult Logout()
        {
            try
            {
                int.TryParse(_contx.HttpContext.Request.Cookies["role"], out int cookievalue);
                if (cookievalue != null)
                {
                    Response.Cookies.Delete("customer");
                    Response.Cookies.Delete("role");
                }

                _contx.HttpContext.Session.Remove("Session");
                _contx.HttpContext.Session.Remove("Count");
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public ActionResult Register(CustomerObject customer)
        {
            try
            {
                acc_repository.AddCustomer(customer);
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult ForgetPassword(string password, string emailSend)
        {
            try
            {
                CustomerObject cus = acc_repository.GetAllCustomer().FirstOrDefault(a => a.Email == emailSend);

                acc_repository.ChangePassword(cus, password);


                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult CheckUsername(string username)
        {
            try
            {
                CustomerObject cus = acc_repository.GetCustomerByUsername(username);

                ManagerObject manager = acc_repository.GetManagerByUsername(username);

                if (cus == null && manager == null)
                {
                    return Content("false");
                }
                return Content("true");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult CheckEmail(string email)
        {
            try
            {
                bool kq = acc_repository.CheckEmail(email);

                if (kq == true)
                {
                    return Content("true");
                }
                return Content("false");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }
    }
}
