using BusinessObject;
using DataAccess.DAOs;
using DataAccess.DataAccess;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging;
using SWP391_Group3_FinalProject.Filter;

namespace GearShop_Customer.Controllers
{
    public class HomeController : Controller
    {
        #region Setting
        IProductRepository pro_repository;
        IOrderRepository ord_repository;
        private readonly IHttpContextAccessor _contx;
        public HomeController(IHttpContextAccessor contx)
        {
            pro_repository = new ProductRepository();
            ord_repository = new OrderRepository();
            _contx = contx;
        }
        #endregion

        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Index()
        {
            try
            {
                List<ProductObject> list = pro_repository.GetAllProducts().ToList();
                List<ProductImageObject> imgs = pro_repository.GetAllProductImages().ToList();

                foreach (var product in list)
                {
                    product.img = imgs
                        .Where(img => img.ProId == product.ProId)
                        .Select(img => img.ProductImage1)
                        .ToList();
                }

                list = list.Where(p => p.ProQuan > 0 && p.IsAvailable == true).ToList();

                List<ProductObject> listMouse = list.Where(pro => pro.CatId == 2 && pro.ProQuan > 0 && (bool)pro.IsAvailable == true).ToList();
                List<ProductObject> listKeyboard = list.Where(pro => pro.CatId == 1 && pro.ProQuan > 0 && (bool)pro.IsAvailable == true).ToList();
                List<BrandObject> brandList = pro_repository.GetAllBrands().Where(b => b.IsAvailable == true).ToList();
                List<CategoryObject> cateList = pro_repository.GetAllCategories().Where(c => c.IsAvailable == true).ToList();

                List<int> totalProductBrand = new List<int>();
                foreach (BrandObject brand in brandList)
                {
                    totalProductBrand.Add(list.Count(pro => pro.BrandId == brand.BrandId));
                }

                List<int> totalProductCate = new List<int>();
                foreach (CategoryObject cate in cateList)
                {
                    totalProductCate.Add(list.Count(pro => pro.CatId == cate.CatId));
                }
                _contx.HttpContext.Session.SetString("listBrand", JsonConvert.SerializeObject(brandList));
                _contx.HttpContext.Session.SetString("listCate", JsonConvert.SerializeObject(cateList));
                _contx.HttpContext.Session.Remove("ErrorLogin");

                ViewBag.totalProductBrand = totalProductBrand;
                ViewBag.totalProductCate = totalProductCate;
                ViewBag.brandList = brandList;
                ViewBag.cateList = cateList;
                ViewBag.listMouse = listMouse;
                ViewBag.listKeyboard = listKeyboard;
                ViewBag.list = list;

                List<Tuple<string, int>> cartCount = new List<Tuple<string, int>>();
                var get = _contx.HttpContext.Session.GetString("Session");
                if (!string.IsNullOrEmpty(get))
                {
                    var cus = JsonConvert.DeserializeObject<Customer>(get);

                    List<CartObject> cartList = ord_repository.GetCartsByUsername(cus.Username).ToList();

                    foreach (var cart in cartList)
                    {
                        Tuple<string, int> tupple = new Tuple<string, int>(cart.ProId, (int)cart.Quantity);
                        cartCount.Add(tupple);
                    }
                }

                ViewBag.cartCount = cartCount;


                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult SearchItem(string searchbox)
        {
            try
            {
                List<ProductObject> list = pro_repository.GetAllProducts().ToList();
                List<ProductImageObject> imgs = pro_repository.GetAllProductImages().ToList();

                foreach (var product in list)
                {
                    product.img = imgs
                        .Where(img => img.ProId == product.ProId)
                        .Select(img => img.ProductImage1)
                        .ToList();
                }

                list = list.Where(p => p.ProQuan > 0 && p.IsAvailable == true).ToList();

                List<ProductObject> foundProducts = new List<ProductObject>();
                if (searchbox != null)
                {
                    foundProducts = list.Where(product => product.ProName.Contains(searchbox, StringComparison.OrdinalIgnoreCase)).ToList();
                }


                return Json(foundProducts);
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

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