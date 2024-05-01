using BusinessObject;
using DataAccess.DAOs;
using DataAccess.DataAccess;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN211_Group3_FinalProject.Filter;

namespace Dashboard.Controllers
{
    [ServiceFilter(typeof(CustomerFilter))]
    public class LoginController : Controller
    {
        #region Setting
        IProductRepository ProRepo;
        IAccountRepository AccountRepo;
        private readonly IHttpContextAccessor _contx;
        private readonly IWebHostEnvironment _environment;
        public LoginController(IHttpContextAccessor contx, IWebHostEnvironment environment)
        {
            ProRepo = new ProductRepository();
            _contx = contx;
            _environment = environment;
            AccountRepo = new AccountRepository();
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verify(ManagerObject manager, string IsRem)
        {
            try
            {
                ManagerObject _manager = AccountRepo.GetManager(manager.Username, manager.Password);
                if (_manager != null)
                {
                    _contx.HttpContext.Session.SetString("SessionAdmin", JsonConvert.SerializeObject(_manager));
                    _contx.HttpContext.Session.SetString("action", JsonConvert.SerializeObject("1"));
                    if (IsRem != null)
                    {
                        HttpContext.Response.Cookies.Append("username", _manager.Username, new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(3),
                        });
                        HttpContext.Response.Cookies.Append("role", "1", new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(3),
                        });
                    }
                    return RedirectToAction("Index", "Dashboard");
                }
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult CheckUsername(string username, string password)
        {
            try
            {
                ManagerObject manager = AccountRepo.GetManager(username, password);


                if (manager == null)
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
    }
}
