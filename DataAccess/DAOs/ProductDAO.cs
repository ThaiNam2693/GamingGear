using AutoMapper;
using BusinessObject;
using DataAccess.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataAccess.DAOs
{
    public class ProductDAO 

    {
        private readonly IMapper _mapper;


        private static ProductDAO instance;

        // Private constructor that accepts IMapper
        private ProductDAO(IMapper mapper)
        {
            _mapper = mapper;
        }

        public static ProductDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())));
                    instance = new ProductDAO(mapper);
                }
                return instance;
            }
        }

        public IEnumerable<ProductObject> GetAllProducts()
        {
            List<ProductObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var products = context.Products.ToList();
                    list = _mapper.Map<List<ProductObject>>(products);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<ProductImageObject> GetAllProductImages()
        {
            List<ProductImageObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var img = context.ProductImages.ToList();
                    list = _mapper.Map<List<ProductImageObject>>(img);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public List<ProductImageObject> GetProductImageBasedOnID()
        {
            List<ProductImageObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    string sqlQuery = "SELECT * FROM Product_Image";

                    // Use Database.SqlQuery to execute the custom SQL query and map the results to your entity or complex type.
                    var listImages = context.ProductImages
                    .FromSql($"SELECT * FROM Product_Image")
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<ProductAttributeObject> GetAllProductAttribute()
        {
            List<ProductAttributeObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var atr = context.ProductAttributes.ToList();
                    list = _mapper.Map<List<ProductAttributeObject>>(atr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public void DeleteImageByPath(string path)
        {
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var imagesToDelete = context.ProductImages
                    .Where(img => img.ProductImage1 == path)
                    .ToList();

                    foreach (var image in imagesToDelete)
                    {
                        context.ProductImages.Remove(image);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public void DeleteAttributeByID(string ID)
        {
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var attributesToDelete = context.ProductAttributes
                        .Where(attr => attr.ProId == ID)
                        .ToList();

                    foreach (var attribute in attributesToDelete)
                    {
                        context.ProductAttributes.Remove(attribute);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or rethrow it if needed
                throw new Exception("Error while deleting product attributes by ID: " + ex.Message, ex);
            }
        }

        public void UpdateProductWithDetails(ProductObject pro)
        {
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var productToUpdate = context.Products.Find(pro.ProId);

                    if (productToUpdate != null)
                    {
                        // Update general product details
                        productToUpdate.ProName = pro.ProName;
                        productToUpdate.BrandId = pro.BrandId;
                        productToUpdate.CatId = pro.CatId;
                        productToUpdate.ProDes = pro.ProDes;
                        productToUpdate.ProPrice = pro.ProPrice;
                        productToUpdate.DiscountPercent = pro.DiscountPercent;

                        // Remove existing product images and attributes
                        

                        // Add new product images
                        foreach (var img in pro.img)
                        {
                            var newProductImage = new ProductImage
                            {
                                ProId = pro.ProId,
                                ProductImage1 = img,
                            };
                            context.ProductImages.Add(newProductImage);
                        }

                        // Add new product attributes
                        foreach (var kvp in pro.pro_attribute)
                        {
                            var newProductAttribute = new ProductAttribute
                            {
                                ProId = pro.ProId,
                                Feature = kvp.Key,
                                Des = kvp.Value
                            };
                            context.ProductAttributes.Add(newProductAttribute);
                        }

                        // Save changes to the database
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or rethrow it if needed
                throw new Exception("Error while updating a product with details: " + ex.Message, ex);
            }
        }



        public IEnumerable<BrandObject> GetAllBrands()
        {
            List<BrandObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var brands = context.Brands.ToList();
                    list = _mapper.Map<List<BrandObject>>(brands);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<CategoryObject> GetAllCategories()
        {
            List<CategoryObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var cates = context.Categories.ToList();
                    list = _mapper.Map<List<CategoryObject>>(cates);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public void UpdateCategory(CategoryObject category)
        {
            try
            {            
                using (var context = new PRNDatabaseContext())
                {
                    var CateToUpdate = context.Categories.Find(category.CatId);
                    if(CateToUpdate != null)
                    {
                        CateToUpdate.CatName = category.CatName;
                        CateToUpdate.CatId = category.CatId;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddCategory(CategoryObject category)
        {
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var newCategory = new Category
                    {
                        CatName = category.CatName,
                        IsAvailable = true,
                        Keyword = category.Keyword,
                    };
                    
                    context.Categories.Add(newCategory);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or rethrow it if neede
                // throw new Exception("Error while adding a category: " + ex.Message, ex);
            }
        }

        public void AddBrand(BrandObject brand)
        {           
                try
                {
                    using (var context = new PRNDatabaseContext())
                    {
                        // Create a new Brand entity and set its properties
                        var newBrand = new Brand
                        {
                            BrandName = brand.BrandName,
                            IsAvailable = true,
                            BrandLogo = brand.BrandLogo
                        };

                        // Add the new brand to the context and save changes to the database
                        context.Brands.Add(newBrand);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception or rethrow it if needed
                    throw new Exception("Error while adding a brand: " + ex.Message, ex);
                }                
        }

        public void EditBrand(BrandObject brand)
        {
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var brandToUpdate = context.Brands.Find(brand.BrandId);
                    if (brandToUpdate != null && brand.BrandLogo != null)
                    {
                        brandToUpdate.BrandName = brand.BrandName;
                        brandToUpdate.BrandLogo = brand.BrandLogo;
                        context.SaveChanges();
                    } else
                    {
                        brandToUpdate.BrandName = brand.BrandName;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or rethrow it if needed
                throw new Exception("Error while editing a brand: " + ex.Message, ex);
            }
        }

        public void AddProduct(ProductObject pro)
        {
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    // Create a new Product entity and set its properties
                    var newProduct = new Product
                    {
                        ProId = pro.ProId,
                        BrandId = pro.BrandId,
                        CatId = pro.CatId,
                        ProName = pro.ProName,  
                        ProQuan = pro.ProQuan,
                        ProDes = pro.ProDes,
                        ProPrice = pro.ProPrice,
                        DiscountPercent = pro.DiscountPercent,  
                        IsAvailable = true
                    };

                    // Add the new product to the context
                    context.Products.Add(newProduct);

                    // Add product images
                    foreach (var img in pro.img)
                    {
                        var productImage = new ProductImage
                        {
                            ProId = pro.ProId,
                            ProductImage1 = img
                        };
                        context.ProductImages.Add(productImage);
                    }

                    // Add product attributes
                    foreach (var kvp in pro.pro_attribute)
                    {
                        var productAttribute = new ProductAttribute
                        {
                            ProId = pro.ProId,
                            Feature = kvp.Key,
                            Des = kvp.Value                            
                        };
                        context.ProductAttributes.Add(productAttribute);
                    }

                    // Save changes to the database
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or rethrow it if needed
                throw new Exception("Error while adding a product with details: " + ex.Message, ex);
            }
        }

        public string GetNewProductID(int cate_id)
        {
            using (var context = new PRNDatabaseContext())
            {
                // Find the last product ID for the given category
                var lastProductId = context.Products
                    .Where(p => p.CatId == cate_id)
                    .OrderByDescending(p => p.ProId)
                    .Select(p => p.ProId)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(lastProductId))
                {
                    // If no products found for the category, get the category's keyword
                    var categoryKeyword = context.Categories
                        .Where(c => c.CatId == cate_id)
                        .Select(c => c.Keyword)
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(categoryKeyword))
                    {
                        // Format the new product ID
                        int newNumber = 1;
                        string newProductId = $"{categoryKeyword}{newNumber:D3}";
                        return newProductId;
                    }
                }
                else
                {
                    // Extract and increment the numeric part of the last product ID
                    string numericPart = lastProductId.Substring(2);
                    if (int.TryParse(numericPart, out int currentNumber))
                    {
                        int newNumber = currentNumber + 1;
                        // Format the new product ID
                        string newProductId = $"{lastProductId.Substring(0, 2)}{newNumber:D3}";
                        return newProductId;
                    }
                }

                return lastProductId;
            }
        }

        public void CreateOrderReciept(ImportReceiptObject IR, List<ReceiptProductObject> RP)
        {
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    // Create and add the Import_Reciept entity
                    var importReceipt = new ImportReceipt
                    {
                        DateImport = IR.DateImport,
                        PersonInCharge = IR.PersonInCharge,
                        Payment = IR.Payment
                    };
                    context.ImportReceipts.Add(importReceipt);
                    context.SaveChanges(); // Save changes to get the generated Receipt_ID

                    int receiptId = importReceipt.ReceiptId;

                    // Create and add Receipt_Product entities
                    foreach (var item in RP)
                    {
                        var receiptProduct = new ReceiptProduct
                        {
                            ReceiptId = receiptId,
                            ProId = item.ProId,
                            ProName = item.ProName,
                            Amount = item.Amount,
                            Price = item.Price
                        };
                        context.ReceiptProducts.Add(receiptProduct);

                        // Update product quantity
                        Product product = context.Products.Find(item.ProId);
                        if (product != null)
                        {
                            product.ProQuan += item.Amount;
                        }
                    }

                    context.SaveChanges(); // Save changes for Receipt_Products and update product quantities
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or rethrow it if needed
                throw new Exception("Error while creating an order receipt: " + ex.Message, ex);
            }
        }

        public void DisableProduct(string ID)
        {
            using (var context = new PRNDatabaseContext())
            {
                var product = context.Products.Find(ID);
                if (product != null)
                {
                    product.IsAvailable = false;
                    context.SaveChanges();
                }
            }
        }

        public void EnableProduct(string ID)
        {
            using (var context = new PRNDatabaseContext())
            {
                var product = context.Products.Find(ID);
                if (product != null)
                {
                    product.IsAvailable = true;
                    context.SaveChanges();
                }
            }
        }

        public void DisableBrand(int ID)
        {
            using (var context = new PRNDatabaseContext())
            {
                // Disable the brand
                var brand = context.Brands.Find(ID);
                if (brand != null)
                {
                    brand.IsAvailable = false;
                    context.SaveChanges();
                }

                // Disable products associated with the brand
                var products = context.Products
                    .Where(product => product.BrandId == ID)
                    .ToList();

                foreach (var product in products)
                {
                    product.IsAvailable = false;
                }

                context.SaveChanges();
            }
        }

        public void EnableBrand(int ID)
        {
            using (var context = new PRNDatabaseContext())
            {
                // Enable the brand
                var brand = context.Brands.Find(ID);
                if (brand != null)
                {
                    brand.IsAvailable = true;
                    context.SaveChanges();
                }

                // Enable products associated with the brand
                var products = context.Products
                    .Where(product => product.BrandId == ID)
                    .ToList();

                foreach (var product in products)
                {
                    product.IsAvailable = true;
                }

                context.SaveChanges();
            }
        }

        public void EnableCategory(int ID)
        {
            using (var context = new PRNDatabaseContext())
            {
                // Enable the category
                var category = context.Categories.Find(ID);
                if (category != null)
                {
                    category.IsAvailable = true;
                    context.SaveChanges();
                }

                // Enable products associated with the category
                var products = context.Products
                    .Where(product => product.CatId == ID)
                    .ToList();

                foreach (var product in products)
                {
                    product.IsAvailable = true;
                }

                context.SaveChanges();
            }
        }


        public void DisableCategory(int ID)
        {
            using (var context = new PRNDatabaseContext())
            {
                // Disable the category
                var category = context.Categories.Find(ID);
                if (category != null)
                {
                    category.IsAvailable = false;
                    context.SaveChanges();
                }

                // Disable products associated with the category
                var products = context.Products
                    .Where(product => product.CatId == ID)
                    .ToList();

                foreach (var product in products)
                {
                    product.IsAvailable = false;
                }

                context.SaveChanges();
            }
        }




    }


}
