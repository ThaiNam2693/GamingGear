using BusinessObject;
using DataAccess.DAOs;
using DataAccess.DataAccess;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.Filter;

namespace GearShop_Customer.Controllers
{
    public class ProductController : Controller
    {
        #region Setting
        IProductRepository pro_repository;
        IOrderRepository ord_repository;
        private readonly IHttpContextAccessor _contx;
        public ProductController(IHttpContextAccessor contx)
        {
            pro_repository = new ProductRepository();
            ord_repository = new OrderRepository();
            _contx = contx;
        }
        #endregion

        // GET: ProductController
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Shop()
        {
            try
            {
                var sortFilter = Request.Query["sort"].ToString();
                var orderFilter = Request.Query["order"].ToString();

                #region get List

                List<ProductObject> list = pro_repository.GetAllProducts().ToList();
                List<ProductImageObject> imgs = pro_repository.GetAllProductImages().ToList();

                foreach (var product in list)
                {
                    product.img = imgs
                        .Where(img => img.ProId == product.ProId)
                        .Select(img => img.ProductImage1)
                        .ToList();
                }
                list = list.OrderBy(p => p.ProQuan == 0 || (bool)!p.IsAvailable ? 1 : 0).ToList();


                var sortCombine = Enumerable.Empty<ProductObject>();
                if (sortFilter.Length > 0 && !sortFilter.Equals(""))
                {
                    var discount = Enumerable.Empty<ProductObject>();
                    var selling = Enumerable.Empty<ProductObject>();
                    if (sortFilter.Contains("discount"))
                    {
                        discount = list.Where(pro => pro.DiscountPercent > 0).ToList();
                    }


                    if (discount.Count() > 0 && selling.Count() > 0)
                    {
                        sortCombine = discount.Intersect(selling);
                    }
                    else if (sortFilter.Contains("discount"))
                    {
                        sortCombine = discount;
                    }
                    else if (sortFilter.Contains("best_selling"))
                    {
                        sortCombine = selling;
                    }
                }

                if (sortCombine.Count() > 0)
                {
                    list = sortCombine.ToList();
                }


                if (orderFilter.Equals("highest"))
                {
                    list = list.OrderByDescending(product => product.ProPrice - (product.ProPrice * product.DiscountPercent) / 100).ToList();
                }
                else if (orderFilter.Equals("lowest"))
                {
                    list = list.OrderBy(product => product.ProPrice - (product.ProPrice * product.DiscountPercent) / 100).ToList();
                }
                List<CategoryObject> cateList = pro_repository.GetAllCategories().Where(c => c.IsAvailable == true).ToList();
                List<BrandObject> brandList = pro_repository.GetAllBrands().Where(b => b.IsAvailable == true).ToList();
                #endregion

                // Lấy trang hiện tại từ query string hoặc mặc định là trang đầu tiên
                var currentPage = Request.Query["page"].ToString() != "" ? Convert.ToInt32(Request.Query["page"]) : 1; ; // Default value

                // Kích thước trang (số sản phẩm mỗi trang)
                int pageSize = 9;

                // Tính chỉ số bắt đầu của sản phẩm cần hiển thị
                int startIndex = (currentPage - 1) * pageSize;

                #region Processing Filter
                var SelectedCategory = Request.Query["category"].ToString().Split(',');
                var SelectedBrand = Request.Query["brand"].ToString().Split(',');
                var selectedCategoryIds = new List<int>();
                var selectedBrandIds = new List<int>();

                //Filter by category
                if (SelectedCategory.Length > 0 && !SelectedCategory.Equals(""))
                {
                    foreach (var item in SelectedCategory)
                    {
                        if (int.TryParse(item, out int parsedCategory))
                        {
                            selectedCategoryIds.Add(parsedCategory);
                        }
                        else
                        {
                            // Handle invalid category value (e.g., log an error, skip, or take appropriate action)
                            Console.WriteLine("Invalid Category: " + item);
                        }
                    }

                }
                var productsByCategory = list.Where(pro => selectedCategoryIds.Contains(pro.CatId)).ToList();

                //Filter by brand
                if (SelectedBrand.Length > 0 && !SelectedBrand.Equals(""))
                {
                    foreach (var item in SelectedBrand)
                    {
                        if (int.TryParse(item, out int parsedCategory))
                        {
                            selectedBrandIds.Add(parsedCategory);
                        }
                        else
                        {
                            // Handle invalid category value (e.g., log an error, skip, or take appropriate action)
                            Console.WriteLine("Invalid Category: " + item);
                        }
                    }

                }
                var productsByBrand = list.Where(pro => selectedBrandIds.Contains(pro.BrandId)).ToList();
                #endregion

                #region Processing list to display
                var productToshow = list.Skip(startIndex).Take(pageSize);
                var combineProduct = Enumerable.Empty<ProductObject>();

                if (selectedCategoryIds.Count() > 0 && selectedBrandIds.Count() > 0)
                {
                    combineProduct = productsByBrand.Intersect(productsByCategory);
                }
                else if (productsByCategory.Count() > 0)
                {
                    combineProduct = productsByCategory;
                }
                else if (productsByBrand.Count() > 0)
                {
                    combineProduct = productsByBrand;
                }


                if (combineProduct.Count() > 0)
                {
                    productToshow = combineProduct.Skip(startIndex).Take(pageSize);
                }
                else if (combineProduct.Count() == 0 && (selectedBrandIds.Count() > 0 || selectedCategoryIds.Count() > 0))
                {
                    productToshow = null;
                }
                #endregion



                int totalItems = selectedCategoryIds.Count == 0 && selectedBrandIds.Count == 0
                         ? list.Count()
                         : combineProduct.Count();

                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                bool isFirstPage = currentPage == 1;
                bool isLastPage = currentPage == totalPages;

                //Get total product by brand
                List<int> totalProductBrand = new List<int>();
                foreach (BrandObject brand in brandList)
                {
                    totalProductBrand.Add(list.Count(pro => pro.BrandId == brand.BrandId));
                }

                //Get total product by category
                List<int> totalProductCate = new List<int>();
                foreach (CategoryObject cate in cateList)
                {
                    totalProductCate.Add(list.Count(pro => pro.CatId == cate.CatId));
                }

                //Get total product on sale
                int totalProductOnsale = list.Count(pro => pro.DiscountPercent > 0);


                #region Set attribute to View Bag
                //Set category list and brand list
                ViewBag.cateList = cateList;
                ViewBag.brandList = brandList;

                //Set category and brand filter used
                ViewBag.selectedCategoryIds = selectedCategoryIds;
                ViewBag.selectedBrandIds = selectedBrandIds;

                //Set page navigation
                ViewBag.currentPage = currentPage;
                ViewBag.pageSize = pageSize;

                //Amount of total product and product filter
                ViewBag.totalProduct = list.Count();
                ViewBag.combineProduct = combineProduct;
                ViewBag.totalProductBrand = totalProductBrand;
                ViewBag.totalProductCate = totalProductCate;
                ViewBag.totalProductOnsale = totalProductOnsale;

                //Store product in list
                ViewBag.combineProduct = combineProduct;
                ViewBag.list = productToshow;

                ViewBag.sort = sortFilter;
                ViewBag.order = orderFilter;
                ViewBag.TotalPages = totalPages;
                ViewBag.IsFirstPage = isFirstPage;
                ViewBag.IsLastPage = isLastPage;
                ViewBag.CurrentPage = currentPage;
                //ViewBag.currentUrl = fullPathWithQuery;
                #endregion

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

        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ShopDetail(string pro_id)
        {
            try
            {
                int cart_quan = 0;
                List<Tuple<string, int>> cartCount = new List<Tuple<string, int>>();


                var get = _contx.HttpContext.Session.GetString("Session");
                if (!string.IsNullOrEmpty(get))
                {
                    var cus = JsonConvert.DeserializeObject<Customer>(get);
                    CartObject c = ord_repository.GetCartsByUsername(cus.Username).FirstOrDefault(p => p.ProId == pro_id);

                    if (c != null)
                    {
                        cart_quan = (int)c.Quantity;
                    }

                    List<CartObject> cartList = ord_repository.GetCartsByUsername(cus.Username).ToList();

                    foreach (var cart in cartList)
                    {
                        Tuple<string, int> tupple = new Tuple<string, int>(cart.ProId, (int)cart.Quantity);
                        cartCount.Add(tupple);
                    }

                }



                int numberOfRandomProducts = 5;
                Random random = new Random();

                List<ProductObject> list = pro_repository.GetAllProducts().ToList();
                List<ProductImageObject> imgs = pro_repository.GetAllProductImages().ToList();
                List<ProductAttributeObject> atr = pro_repository.GetAllProductAttribute().ToList();
                foreach (var product in list)
                {
                    product.img = imgs
                        .Where(img => img.ProId == product.ProId)
                        .Select(img => img.ProductImage1)
                        .ToList();
                }

                foreach (var product in list)
                {
                    var attributes = atr
                        .Where(atr => atr.ProId == product.ProId)
                        .GroupBy(atr => atr.Feature)
                        .ToDictionary(group => group.Key, group => group.Last().Des);

                    product.pro_attribute = attributes;
                }
                List<BrandObject> brandList = pro_repository.GetAllBrands().ToList();

                ProductObject pro1 = list.FirstOrDefault(p => p.ProId == pro_id);

                List<ProductObject> randomProducts = new List<ProductObject>();
                for (int i = 0; i < numberOfRandomProducts && list.Count > 0; i++)
                {
                    int randomIndex = random.Next(0, list.Count);
                    ProductObject randomProduct = list[randomIndex];
                    randomProducts.Add(randomProduct);
                    //remove duplicated item.
                    list.RemoveAt(randomIndex);
                }
                List<ProductObject> productByCateList = list.Where(pro => pro.CatId == pro1.CatId && pro.ProQuan > 0 && pro.IsAvailable == true).ToList();
                ViewBag.pro = pro1;
                ViewBag.productByCateList = productByCateList;
                ViewBag.brandList = brandList;
                ViewBag.RandomProducts = randomProducts;

                ViewBag.cartQuan = cart_quan;

                ViewBag.cartCount = cartCount;

                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }


        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ShopSearch(string searchTerm)
        {
            try
            {
                List<ProductObject> searchList = pro_repository.GetAllProducts().ToList();
                List<ProductImageObject> imgs = pro_repository.GetAllProductImages().ToList();
                foreach (var product in searchList)
                {
                    product.img = imgs
                        .Where(img => img.ProId == product.ProId)
                        .Select(img => img.ProductImage1)
                        .ToList();
                }

                searchList = searchList.OrderBy(p => p.ProQuan == 0 || (bool)!p.IsAvailable ? 1 : 0).ToList();

                List<ProductObject> foundProducts = new List<ProductObject>();
                // Lấy trang hiện tại từ query string hoặc mặc định là trang đầu tiên
                var currentPage = Request.Query["page"].ToString() != "" ? Convert.ToInt32(Request.Query["page"]) : 1; ; // Default value


                if (searchTerm != null)
                {
                    foundProducts = searchList.Where(product => product.ProName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Kích thước trang (số sản phẩm mỗi trang)
                int pageSize = 12;

                // Tính chỉ số bắt đầu của sản phẩm cần hiển thị
                int startIndex = (currentPage - 1) * pageSize;

                int totalItems = foundProducts.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                bool isFirstPage = currentPage == 1;
                bool isLastPage = currentPage == totalPages;

                var productToshow = foundProducts.Skip(startIndex).Take(pageSize).ToList();


                ViewBag.foundProducts = productToshow;
                ViewBag.searchterm = searchTerm;
                //Set page navigation
                ViewBag.currentPage = currentPage;
                ViewBag.pageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.IsFirstPage = isFirstPage;
                ViewBag.IsLastPage = isLastPage;
                ViewBag.CurrentPage = currentPage;

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

    }
}
