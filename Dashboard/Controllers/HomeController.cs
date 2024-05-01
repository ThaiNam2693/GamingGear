using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public IActionResult Index()
        {
            return View();
        }

        [Route("/StatusCodeError/{statusCode}")]
        public IActionResult Error(int statusCode)
        {

            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = "Page not found";
            }
            return View();

        }
    }
}