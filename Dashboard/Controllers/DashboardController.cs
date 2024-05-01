using BusinessObject;
using DataAccess.DAOs;
using DataAccess.DataAccess;
using DataAccess.IRepository;
using DataAccess.Repository;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework.Profiler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SWP391_Group3_FinalProject.Controllers
{
    //[ServiceFilter(typeof(LoginFilter))]
    //[ServiceFilter(typeof(ManagerFilter))]
    public class DashboardController : Controller
    {

        #region Setting
        IProductRepository ProRepo;
        IOrderRepository OrderRepo;
        IAccountRepository AccountRepo;
        IManagerRepository ManagerRepo;
        private readonly IHttpContextAccessor _contx;
        private readonly IWebHostEnvironment _environment;
        public DashboardController(IHttpContextAccessor contx, IWebHostEnvironment environment)
        {
            ProRepo = new ProductRepository();
            OrderRepo = new OrderRepository();
            AccountRepo = new AccountRepository();
            ManagerRepo = new ManagerRepository();
            _contx = contx;
            _environment = environment;
        }
        #endregion


        public IActionResult Index()
        {
            try
            {
                //Out Of Stock Product
                var filteredProducts = ProRepo.GetAllProducts().Where(pro => pro.ProQuan == 1 || pro.ProQuan == 0).ToList();


                DateTime currentDate = DateTime.Now;
                int currentYear = currentDate.Year;
                int currentMonth = currentDate.Month;
                List<OrderObject> orders = OrderRepo.GetAllOrders().ToList();
                List<OrderDetailObject> orderDetails = OrderRepo.GetAllOrderDetail().ToList();
                List<CustomerObject> customers = AccountRepo.GetAllCustomer().ToList();

                //Completed Order
                var CompletedOrder = orders.Where(o => o.Status == 4 && o.EndDate != null &&
                o.EndDate.Value.Year == currentYear && o.EndDate.Value.Month == currentMonth).ToList();
                int CompletedOrderCount = CompletedOrder.Count();


                //Monthly Income
                var IncomeList = orders.Where(o => o.Status == 4 && o.EndDate != null &&
                o.EndDate.Value.Year == currentYear && o.EndDate.Value.Month == currentMonth).Select(o => o.TotalPrice).ToList();
                decimal TotalIncome = IncomeList.Sum().Value;


                //Top 10 Products
                var consolidatedProducts = orderDetails
                 .GroupBy(detail => detail.ProId)
                  .Select(group => new
                  {
                      productID = group.Key,
                      productName = group.First().ProName, // Take the product name from the first item in the group
                      quantity = group.Sum(detail => detail.Quantity)
                  })
                .OrderByDescending(product => product.quantity) // Sort by quantity in descending order
                .Take(10) // Select the top 10 products
                .ToList();

                //Top 10 Customers
                var query = from order in orders
                            where order.Status == 4
                            join customer in customers on order.Username equals customer.Username
                            group order by customer.Fullname into grouped
                            select new
                            {
                                Fullname = grouped.Key,
                                TotalPriceSum = grouped.Sum(o => o.TotalPrice)
                            };
                var resultList = query.ToList();

                //ViewBag
                ViewData["TotalIncome"] = TotalIncome;
                ViewData["CompletedOrder"] = CompletedOrderCount;
                ViewBag.LowQuantityProduct = filteredProducts;
                ViewBag.Top10Product = consolidatedProducts;
                ViewBag.Top10Customer = resultList;

                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult ProductPage()
        {
            try
            {
                //Get List for Page
                List<BrandObject> BrandList = ProRepo.GetAllBrands().ToList();
                List<CategoryObject> CategoryList = ProRepo.GetAllCategories().ToList();
                List<ProductObject> ProductList = ProRepo.GetAllProducts().ToList();


                //ViewBag
                ViewBag.BrandList = BrandList;
                ViewBag.CategoryList = CategoryList;
                ViewBag.ProductList = ProductList.Where(pro => pro.IsAvailable == true).ToList();
                ViewBag.ProductListDisable = ProductList.Where(pro => pro.IsAvailable == false).ToList();
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult GetCategoryInfo(int cate_id)
        {

            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                CategoryObject Cate = ProRepo.GetAllCategories().FirstOrDefault(c => c.CatId == cate_id);
                if (Cate != null)
                {
                    return Ok(Cate); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("Category not found");
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("/StatusCodeError");

            }
        }

        [HttpPost]
        public IActionResult GetBrandInfo(int brand_id)
        {

            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                BrandObject Brand = ProRepo.GetAllBrands().FirstOrDefault(b => b.BrandId == brand_id);

                if (Brand != null)
                {
                    Console.WriteLine(Brand);
                    return Ok(Brand); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("Brand not found");
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("/StatusCodeError");

            }

        }

        public IActionResult UpdateCategory(CategoryObject category)
        {
            try
            {
                ProRepo.UpdateCategory(category);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Update Category with ID " + category.CatId + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }


        }

        public IActionResult AddCategory(CategoryObject category)
        {
            try
            {
                ProRepo.AddCategory(category);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Category with name " + category.CatId + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult AddBrand(BrandObject brand, IFormFile BrandLogo)
        {
            try
            {
                List<BrandObject> BrandList = ProRepo.GetAllBrands().ToList();
                int highestBrandId = BrandList.Max(b => b.BrandId);
                int nextBrandID = highestBrandId + 1;

                if (BrandLogo != null && BrandLogo.Length > 0)
                {
                    try
                    {
                        var uniqueFileName = nextBrandID + "_Logo" + Path.GetExtension(BrandLogo.FileName); ;

                        // Define the path where the file will be saved on the server.
                        var webRootPath = _environment.WebRootPath;
                        var uploadPath = Path.Combine(webRootPath, "source_img", "brand_logo");
                        var filePath = Path.Combine(_environment.WebRootPath, "source_img", "brand_logo", uniqueFileName);


                        string originalPath = filePath;
                        string replacedPath = originalPath.Replace("Dashboard", "GearShop_Customer");

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }


                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            // Copy the uploaded file's content to the stream.
                            BrandLogo.CopyTo(stream);
                        }

                        using (var stream = new FileStream(replacedPath, FileMode.Create))
                        {
                            // Copy the uploaded file's content to the stream.
                            BrandLogo.CopyTo(stream);
                        }

                        // Create a URL to access the saved file.
                        var imageUrl = "\\source_img\\brand_logo\\" + uniqueFileName;

                        // Now, imageUrl can be used as the source in your HTML.
                        brand.BrandLogo = imageUrl;
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur during file upload or processing.
                        // You can add logging or return an error response to the client.
                    }

                    ProRepo.AddBrand(brand);
                }


                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Brand with name " + brand.BrandId + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult UpdateBrand(BrandObject brand, IFormFile BrandLogo)
        {
            try
            {
                if (BrandLogo != null && BrandLogo.Length > 0)
                {
                    try
                    {
                        var uniqueFileName = brand.BrandId + "_Logo" + Path.GetExtension(BrandLogo.FileName); ;

                        // Define the path where the file will be saved on the server.
                        var webRootPath = _environment.WebRootPath;
                        var uploadPath = Path.Combine(webRootPath, "source_img", "brand_logo");
                        var filePath = Path.Combine(_environment.WebRootPath, "source_img", "brand_logo", uniqueFileName);

                        string originalPath = filePath;
                        string replacedPath = originalPath.Replace("Dashboard", "GearShop_Customer");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }


                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            // Copy the uploaded file's content to the stream.
                            BrandLogo.CopyTo(stream);
                        }

                        using (var stream = new FileStream(replacedPath, FileMode.Create))
                        {
                            // Copy the uploaded file's content to the stream.
                            BrandLogo.CopyTo(stream);
                        }

                        // Create a URL to access the saved file.
                        var imageUrl = "\\source_img\\brand_logo\\" + uniqueFileName;

                        // Now, imageUrl can be used as the source in your HTML.
                        brand.BrandLogo = imageUrl;
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur during file upload or processing.
                        // You can add logging or return an error response to the client.
                    }

                }

                ProRepo.UpdateBrand(brand);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Brand with ID " + brand.BrandId + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult AddProduct(ProductObject pro, List<IFormFile> imgFile, List<string> feature, List<string> description)
        {
            try
            {
                CategoryObject cate = ProRepo.GetAllCategories().FirstOrDefault(c => c.CatId == pro.CatId);
                string folder = cate.CatName.Trim();

                var webRootPath = _environment.WebRootPath;
                var uploadPath = Path.Combine(webRootPath, "source_img", "product_image", folder);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                int index = 1;
                foreach (var image in imgFile)
                {
                    if (image != null && image.Length > 0)
                    {

                        string fileName = pro.ProId + "_" + index + Path.GetExtension(image.FileName);
                        string PathIntoDatabase = Path.Combine("\\" + "source_img", "product_image", folder, fileName);
                        string filePath = Path.Combine(_environment.WebRootPath, "source_img", "product_image", folder, fileName);
                        string originalPath = filePath;
                        string replacedPath = originalPath.Replace("Dashboard", "GearShop_Customer");

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        using (var stream = new FileStream(replacedPath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        pro.img.Add(PathIntoDatabase);
                        index++;
                    }
                }

                int count = feature.Count();
                for (int i = 0; i < count; i++)
                {
                    pro.pro_attribute[feature[i]] = description[i];
                }

                ProRepo.AddProduct(pro);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Product with ID " + pro.ProId + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult GetNewProductID(int cate_id)
        {
            try
            {
                string newID = ProRepo.GetNewProductID(cate_id);
                return Content(newID);
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult Logout()
        {
            try
            {
                string ManagerInfo, role;
                _contx.HttpContext.Session.Remove("SessionAdmin");
                _contx.HttpContext.Session.Remove("action");
                int cookievalue = 0;
                if (_contx.HttpContext.Request.Cookies["role"] != null)
                {
                    cookievalue = int.Parse(_contx.HttpContext.Request.Cookies["role"]);
                }
                if (cookievalue != null)
                {
                    Response.Cookies.Delete("username");
                    Response.Cookies.Delete("role");
                }
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult ProductDetailPage(string ID)
        {
            try
            {
                List<BrandObject> BrandList = ProRepo.GetAllBrands().ToList();
                List<CategoryObject> CategoryList = ProRepo.GetAllCategories().ToList();
                List<ProductObject> list = ProRepo.GetAllProducts().ToList();
                List<ProductImageObject> imgs = ProRepo.GetAllProductImages().ToList();
                List<ProductAttributeObject> atr = ProRepo.GetAllProductAttribute().ToList();
                foreach (var product in list)
                {
                    product.img = imgs
                        .Where(img => img.ProId == product.ProId)
                        .Select(img => img.ProductImage1)
                        .ToList();
                }
                ProductObject pro = list.FirstOrDefault(p => p.ProId == ID);
                //ViewBag
                ViewBag.ProductAttribute = ProRepo.GetAllProductAttribute().Where(p => p.ProId.Equals(ID));
                ViewBag.ProductImage = pro.img;
                ViewBag.CategoryList = CategoryList.Where(c => c.IsAvailable == true).ToList();
                ViewBag.BrandList = BrandList.Where(b => b.IsAvailable == true).ToList();
                return View(pro);
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult EditProduct(List<int> Image_ID, ProductObject pro, List<string> feature, List<string> description, List<IFormFile> imgFile, string selectedImages, string ImagesList)
        {
            try
            {
                List<string> imageList = ImagesList.Split(',').ToList();
                if (selectedImages != null)
                {
                    List<string> selectedImageList = JsonConvert.DeserializeObject<List<string>>(selectedImages);
                    foreach (var path in selectedImageList)
                    {
                        var webRootPath = _environment.WebRootPath;
                        string filePath = webRootPath + path;
                        string originalPath = filePath;
                        string replacedPath = originalPath.Replace("Dashboard", "GearShop_Customer");
                        try
                        {
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                                ProRepo.DeleteImageByPath(path);
                            }

                            if (System.IO.File.Exists(replacedPath))
                            {
                                System.IO.File.Delete(replacedPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            return Content("Error: " + ex.Message);
                        }

                    }
                }



                CategoryObject cate = ProRepo.GetAllCategories().FirstOrDefault(c => c.CatId == pro.CatId);
                string folder = cate.CatName.Trim();
                List<IFormFile> file = new List<IFormFile>();
                List<string> name = new List<string>();


                List<ProductImageObject> imgs = ProRepo.GetAllProductImages().ToList();

                int count = imgs.Count(img => img.ProId == pro.ProId);
                for (int i = 0; i < count; i++)
                {
                    var Images = Request.Form.Files[pro.ProId + "_" + Image_ID[i]];
                    name.Add(pro.ProId + "_" + Image_ID[i]);
                    file.Add(Images);

                }


                for (int i = 0; i <= count - 1; i++)
                {
                    var Image = file[i];

                    var OriginalImage = imageList[i];

                    if (Image != null && Image.Length > 0)
                    {
                        try
                        {
                            var uniqueFileName = name[i] + Path.GetExtension(file[i].FileName);
                            string fileExtension = Path.GetExtension(uniqueFileName);
                            var webRootPath = _environment.WebRootPath;
                            var uploadPath2 = Path.Combine("\\" + "source_img", "product_image", folder, uniqueFileName);
                            var uploadPath = Path.Combine(_environment.WebRootPath, "source_img", "product_image", folder, uniqueFileName);
                            string filePath = webRootPath + OriginalImage;
                            string originalPath = filePath;
                            string replacedPath = originalPath.Replace("Dashboard", "GearShop_Customer");
                            try
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                    ProRepo.DeleteImageByPath(OriginalImage);
                                    pro.img.Add(uploadPath2);
                                }
                                if (System.IO.File.Exists(replacedPath))
                                {
                                    System.IO.File.Delete(replacedPath);
                                }
                            }
                            catch (Exception ex)
                            {
                                return Content("Error: " + ex.Message);
                            }
                            using (var stream = new FileStream(uploadPath, FileMode.Create))
                            {
                                // Copy the uploaded file's content to the stream.
                                Image.CopyTo(stream);
                            }
                            using (var stream = new FileStream(replacedPath, FileMode.Create))
                            {
                                // Copy the uploaded file's content to the stream.
                                Image.CopyTo(stream);
                            }

                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions that may occur during file upload or processing.
                            // You can add logging or return an error response to the client.
                        }
                    }

                }

                int index;

                if (Image_ID.Any()) // Check if the sequence contains any elements
                {
                    index = Image_ID.Max() + 1;
                }
                else
                {
                    // The sequence is empty, so set index to 1 or any other default value as needed
                    index = 1;
                }
                foreach (var image in imgFile)
                {
                    if (image != null && image.Length > 0)
                    {

                        string fileName = pro.ProId + "_" + index + Path.GetExtension(image.FileName);
                        string PathIntoDatabase = Path.Combine("\\" + "source_img", "product_image", folder, fileName);
                        string filePath = Path.Combine(_environment.WebRootPath, "source_img", "product_image", folder, fileName);
                        string originalPath = filePath;
                        string replacedPath = originalPath.Replace("Dashboard", "GearShop_Customer");

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        using (var stream = new FileStream(replacedPath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        pro.img.Add(PathIntoDatabase);
                        index++;
                    }
                }

                int counter = feature.Count();
                for (int i = 0; i < counter; i++)
                {
                    pro.pro_attribute[feature[i]] = description[i];
                }


                ProRepo.DeleteAttributeByID(pro.ProId);
                ProRepo.UpdateProduct(pro);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Update Product with ID " + pro.ProId + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }


        }


        [HttpPost]
        public IActionResult KeywordExisted(string keyword)
        {
            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                List<CategoryObject> list = ProRepo.GetAllCategories().ToList();
                CategoryObject cat = list.FirstOrDefault(c => c.Keyword.Equals(keyword));
                if (cat != null)
                {
                    return Content("Existed");
                }
                else
                {
                    return Content("NotExisted");
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("/StatusCodeError");

            }
        }

        public IActionResult ImportProduct()
        {
            try
            {
                List<BrandObject> BrandList = ProRepo.GetAllBrands().ToList();
                List<CategoryObject> CategoryList = ProRepo.GetAllCategories().ToList();
                List<ProductObject> list = ProRepo.GetAllProducts().ToList();

                // Use LINQ to filter products with quantity equal to 0 or 1
                var filteredProducts = list.Where(product => product.ProQuan == 0 || product.ProQuan == 1).ToList();
                // Use LINQ to filter products with quantity greater than 1
                var filteredProductsGreaterThanOne = list
                    .Where(product => product.ProQuan > 1)
                    .ToList();
                //View Bag
                ViewBag.CategoryList = CategoryList;
                ViewBag.BrandList = BrandList;
                ViewBag.LowQuantityProduct = filteredProducts;
                ViewBag.NormalProduct = filteredProductsGreaterThanOne;


                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult GetImportProduct(int totalCartPriceNumber)
        {
            try
            {
                var ProductIDs = Request.Form["pro_id"];
                var ProductNames = Request.Form["pro_name"];
                var Quantities = Request.Form["amount"];
                var Prices = Request.Form["price"];

                var ProductImported = new List<ReceiptProductObject>();

                //Get all the product into recieptProduct
                for (int i = 0; i < ProductIDs.Count; i++)
                {
                    var recieptProduct = new ReceiptProductObject
                    {
                        ProId = ProductIDs[i].ToString().Trim(),
                        ProName = ProductNames[i].ToString().Trim(),
                        Amount = Convert.ToInt32(Quantities[i].ToString().Trim()),
                        Price = Convert.ToInt32(Prices[i].ToString().Trim())

                    };
                    ProductImported.Add(recieptProduct);
                }
                string Manager = _contx.HttpContext.Session.GetString("SessionAdmin");
                ManagerObject manager = new ManagerObject();
                if (Manager != null)
                {
                    manager = JsonConvert.DeserializeObject<ManagerObject>(Manager);
                }
                //Create an Import_Reciept
                ImportReceiptObject IR = new ImportReceiptObject
                {
                    DateImport = DateTime.Now,
                    PersonInCharge = manager.Fullname,
                    Payment = totalCartPriceNumber
                };
                ProRepo.CreateOrderReciept(IR, ProductImported);



                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }
        public IActionResult ImportReceipts()
        {
            try
            {
                List<ImportReceiptObject> IRList = OrderRepo.GetAllImportReceipt();
                //ViewBag
                ViewBag.IRList = IRList;
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult GetIRInfo(int ID)
        {
            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                ImportReceiptObject importReciept = OrderRepo.GetAllImportReceipt().FirstOrDefault(o => o.ReceiptId == ID);

                if (importReciept != null)
                {
                    Console.WriteLine(importReciept);
                    return Ok(importReciept); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("IR not found");
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("/StatusCodeError");

            }
        }

        [HttpPost]
        public IActionResult GetRPInfo(int ID)
        {
            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                ImportReceiptObject importReciept = OrderRepo.GetAllImportReceipt().FirstOrDefault(o => o.ReceiptId == ID);
                List<ReceiptProductObject> list = OrderRepo.GetAllReceiptProduct().Where(o => o.ReceiptId == ID).ToList();

                if (list != null)
                {
                    Console.WriteLine(list);
                    return Ok(list); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("IR not found");
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("/StatusCodeError");

            }
        }

        public IActionResult PersonalProfile()
        {
            return View();
        }

        [HttpPost]
        //Update staff information
        public IActionResult PersonalProfile(ManagerObject staff)
        {
            try
            {
                ManagerRepo.UpdateStaffAccount(staff);
                ManagerObject manager = AccountRepo.GetManagerByUsername(staff.Username);
                manager.Email = staff.Email;
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Your profile has been successfully updated!"));
                _contx.HttpContext.Session.SetString("SessionAdmin", JsonConvert.SerializeObject(manager));
                return RedirectToAction("PersonalProfile", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        //Coi đơn hàng của khách hàng

        public IActionResult OrderRecieptPage()
        {
            try
            {
                IEnumerable<OrderAddressObject> addlist = OrderRepo.GetAllOrderAddress();
                IEnumerable<OrderObject> list = OrderRepo.GetAllOrder();
                ViewBag.Addresses = addlist.ToList();
                ViewBag.OrderPending = list.Where(o => o.Status == 1).ToList();
                ViewBag.OrderAccepted = list.Where(o => o.Status == 2).ToList();
                ViewBag.OrderShipped = list.Where(o => o.Status == 3).ToList();
                ViewBag.OrderCompleted = list.Where(o => o.Status == 4).ToList();
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }
        }
        //Update order status
        public IActionResult AcceptOrder(string ID)
        {
            var serializedmanager = _contx.HttpContext.Session.GetString("SessionAdmin");
            var manager = JsonConvert.DeserializeObject<Manager>(serializedmanager);
            OrderRepo.AcceptOrder(ID, manager.Id);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " accepted by " + manager.Fullname));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }
        public IActionResult CancelOrder(string ID)
        {
            //chuyển status = 0
            OrderRepo.CancelOrder(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " has been cancelled. Products from order have been sent back to store"));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }
        public IActionResult ShippedOrder(string ID)
        {
            //Gắn End_Date là date lúc bấm nút
            //status = 3
            OrderRepo.ShippedOrder(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " shipped on " + DateTime.Now.ToString("dd/MM/yyyy")));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }
        public IActionResult CompletedOrder(string ID)
        {
            //chuyển status = 4
            OrderRepo.CompletedOrder(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " has been delivered"));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }

        [HttpPost]
        public IActionResult GetOrderInfo(string ID)
        {
            try
            {
                OrderObject order = OrderRepo.GetAllOrder().SingleOrDefault(o => o.OrderId.Equals(ID));
                OrderAddressObject address = OrderRepo.GetOrderAddress(ID);
                CustomerObject cus = AccountRepo.GetCustomerByUsername(order.Username);
                IEnumerable<OrderDetailObject> list = OrderRepo.GetOrderDetail(ID);
                ManagerObject staff = ManagerRepo.GetAllManagers().FirstOrDefault(m => m.Id == order.StaffId);
                if (order != null)
                {
                    var result = new
                    {
                        Order = order,
                        Address = address,
                        Email = cus.Email,
                        OrderDetail = list,
                        Staff = staff,
                    };
                    Console.WriteLine(order);
                    return Ok(result); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("Order not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public IActionResult DisableProduct(string ID)
        {
            try
            {
                ProRepo.DisableProduct(ID);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Disable Product with ID " + ID + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult EnableProduct(string ID)
        {
            try
            {
                ProRepo.EnableProduct(ID);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Enable Product with ID " + ID + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult DisableBrand(int ID)
        {
            try
            {
                ProRepo.DisableBrand(ID);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Disable Brand with ID " + ID + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult DisableCategory(int ID)
        {
            try
            {
                ProRepo.DisableCategory(ID);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Disable Category with ID " + ID + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult EnableBrand(int ID)
        {
            try
            {
                ProRepo.EnableBrand(ID);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Enable Brand with ID " + ID + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        public IActionResult EnableCategory(int ID)
        {
            try
            {
                ProRepo.EnableCategory(ID);
                _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Enable Category with ID " + ID + " Successfully"));
                return RedirectToAction("ProductPage", "Dashboard");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }


        [HttpPost]
        public IActionResult CheckEmailUpdate(string email)
        {
            try
            {
                ManagerObject manager = AccountRepo.GetAllManager().FirstOrDefault(m => m.Email.Equals(email));
                List<CustomerObject> listCus = AccountRepo.GetAllCustomer().ToList();
                var serializedManager = _contx.HttpContext.Session.GetString("SessionAdmin");
                var currentManager = JsonConvert.DeserializeObject<Manager>(serializedManager);

                if (currentManager != null)
                {
                    List<ManagerObject> list = AccountRepo.GetAllManager().ToList();
                    list = list.Where(m => !string.Equals(m.Email, currentManager.Email, StringComparison.OrdinalIgnoreCase)).ToList();

                    bool isManagerEmailExisted1 = list.Any(m => string.Equals(m.Email, email, StringComparison.OrdinalIgnoreCase));
                    bool isManagerEmailExisted2 = listCus.Any(m => string.Equals(m.Email, email, StringComparison.OrdinalIgnoreCase));
                    if (isManagerEmailExisted1 == true || isManagerEmailExisted2 == true)
                    {
                        return Content("Existed"); // Return a 200 OK response with JSON data
                    }
                    else
                    {
                        return Content("No");
                    }
                }

                return NotFound("Brand not found");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }


        }
    }

}
