using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<BrandObject> GetAllBrands()=>ProductDAO.Instance.GetAllBrands();

        public IEnumerable<CategoryObject> GetAllCategories()=>ProductDAO.Instance.GetAllCategories();

        public IEnumerable<ProductImageObject> GetAllProductImages()=>ProductDAO.Instance.GetAllProductImages();

        public IEnumerable<ProductAttributeObject> GetAllProductAttribute()=>ProductDAO.Instance.GetAllProductAttribute();

        public IEnumerable<ProductObject> GetAllProducts()=>ProductDAO.Instance.GetAllProducts();

        public void UpdateCategory(CategoryObject cate) => ProductDAO.Instance.UpdateCategory(cate);

        public void AddCategory(CategoryObject cate) => ProductDAO.Instance.AddCategory(cate);

        public void AddBrand(BrandObject brand ) => ProductDAO.Instance.AddBrand(brand);

        public void UpdateBrand(BrandObject brand) => ProductDAO.Instance.EditBrand(brand);

        public void AddProduct(ProductObject pro) => ProductDAO.Instance.AddProduct(pro);

        public string GetNewProductID(int cate_id) => ProductDAO.Instance.GetNewProductID(cate_id);

        public void DeleteImageByPath(string path) => ProductDAO.Instance.DeleteImageByPath(path);

        public void DeleteAttributeByID(string ID) => ProductDAO.Instance.DeleteAttributeByID(ID);
        
        public void UpdateProduct(ProductObject pro) => ProductDAO.Instance.UpdateProductWithDetails(pro);

        public void CreateOrderReciept(ImportReceiptObject IR, List<ReceiptProductObject> RP) => ProductDAO.Instance.CreateOrderReciept(IR, RP);

        public void DisableProduct(string ID) => ProductDAO.Instance.DisableProduct(ID);
        

        public void EnableProduct(string ID) => ProductDAO.Instance.EnableProduct(ID);

        public void DisableBrand(int ID) =>ProductDAO.Instance.DisableBrand(ID);    
        

        public void EnableBrand(int ID) => ProductDAO.Instance.EnableBrand(ID);
        

        public void EnableCategory(int ID) => ProductDAO.Instance.EnableCategory(ID);
        

        public void DisableCategory(int ID) => ProductDAO.Instance.DisableCategory(ID);
        
    }
}
