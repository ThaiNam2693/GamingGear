using AutoMapper;
using BusinessObject;


namespace DataAccess.DataAccess
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Brand, BrandObject>().ReverseMap();
            CreateMap<Cart, CartObject>().ReverseMap();
            CreateMap<Category, CategoryObject>().ReverseMap();
            CreateMap<Customer, CustomerObject>().ReverseMap();
            CreateMap<DeliveryAddress, DeliveryAddressObject>().ReverseMap();
            CreateMap<ImportReceipt, ImportReceiptObject>().ReverseMap();
            CreateMap<Manager, ManagerObject>().ReverseMap();
            CreateMap<Order, OrderObject>().ReverseMap();
            CreateMap<OrderAddress, OrderAddressObject>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailObject>().ReverseMap();
            CreateMap<Product, ProductObject>().ReverseMap();
            CreateMap<ProductAttribute, ProductAttributeObject>().ReverseMap();
            CreateMap<ProductImage, ProductImageObject>().ReverseMap();
            CreateMap<ReceiptProduct, ReceiptProductObject>().ReverseMap();
        }
    }
}
