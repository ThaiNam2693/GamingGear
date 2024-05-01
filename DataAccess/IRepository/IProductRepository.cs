using BusinessObject;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IProductRepository
    {
        IEnumerable<ProductObject> GetAllProducts();
        IEnumerable<ProductImageObject> GetAllProductImages();
        IEnumerable<ProductAttributeObject> GetAllProductAttribute();
        IEnumerable<BrandObject> GetAllBrands();
        IEnumerable<CategoryObject> GetAllCategories();

        public void UpdateCategory(CategoryObject cate);

        public void AddCategory(CategoryObject cate);

        public void AddBrand(BrandObject brand);

        public void UpdateBrand(BrandObject brand);

        public void AddProduct(ProductObject pro);

        public string GetNewProductID(int cate_id);

        public void DeleteImageByPath(string path);

        public void DeleteAttributeByID(string ID);
        public void UpdateProduct(ProductObject pro);

        public void CreateOrderReciept(ImportReceiptObject IR, List<ReceiptProductObject> RP);

        public void DisableProduct(string ID);
        public void EnableProduct(string ID);
        public void DisableBrand(int ID);
        public void EnableBrand(int ID);
        public void EnableCategory(int ID);
        public void DisableCategory(int ID);


    }
}
