using AutoMapper;
using BusinessObject;
using DataAccess.DataAccess;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class OrderDAO
    {
        private readonly IMapper _mapper;

        private static OrderDAO instance;

        // Private constructor that accepts IMapper
        private OrderDAO(IMapper mapper)
        {
            _mapper = mapper;
        }

        public static OrderDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())));
                    instance = new OrderDAO(mapper);
                }
                return instance;
            }
        }

        public IEnumerable<OrderObject> GetAllOrder()
        {
            List<OrderObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var c = context.Orders.ToList();
                    list = _mapper.Map<List<OrderObject>>(c);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<OrderDetailObject> GetAllOrderDetail()
        {
            List<OrderDetailObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var c = context.OrderDetails.ToList();
                    list = _mapper.Map<List<OrderDetailObject>>(c);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<CartObject> GetCartsByUsername(string username)
        {
            List<CartObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var c = context.Carts.Where(c => c.Username == username);
                    list = _mapper.Map<List<CartObject>>(c);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<OrderObject> GetOrdersByUsername(string username)
        {
            List<OrderObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var ords = context.Orders.Where(c => c.Username == username);
                    list = _mapper.Map<List<OrderObject>>(ords);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<OrderDetailObject> GetOrderDetail(string ordID)
        {
            List<OrderDetailObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var ords = context.OrderDetails.Where(c => c.OrderId == ordID);
                    list = _mapper.Map<List<OrderDetailObject>>(ords);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public OrderAddressObject GetOrderAddress(string ordID)
        {
            OrderAddressObject list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var ords = context.OrderAddresses.SingleOrDefault(c => c.OrderId == ordID);
                    list = _mapper.Map<OrderAddressObject>(ords);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }
        public IEnumerable<OrderAddressObject> GetAllOrderAddress()
        {
            List<OrderAddressObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var addresses = context.OrderAddresses.ToList();
                    list = _mapper.Map<List<OrderAddressObject>>(addresses);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public CartObject GetCartByProID(string Id)
        {
            CartObject cart = null;

            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var _cart = context.Carts.SingleOrDefault(c => c.ProId == Id);
                    cart = _mapper.Map<CartObject>(_cart);
                }
            }
            catch
            {

            }
            return cart;
        }

        public void AddCart(CartObject c)
        {
            IProductRepository productRepository = new ProductRepository();
            List<ProductObject> listpro = productRepository.GetAllProducts().ToList();

            var pro = listpro.FirstOrDefault(p => p.ProId == c.ProId);
            if (pro.ProQuan <= 0 || pro.ProQuan < c.Quantity)
            {
                return;
            }

            Cart _c = _mapper.Map<Cart>(c);
            using (var context = new PRNDatabaseContext())
            {
                context.Carts.Add(_c);
                context.SaveChanges();
            }
        }

        public void DeleteCart(string us, string proID)
        {
            try
            {
                CartObject c = GetCartsByUsername(us).SingleOrDefault(c => c.ProId == proID);
                Cart _c = _mapper.Map<Cart>(c);
                using (var context = new PRNDatabaseContext())
                {
                    context.Carts.Remove(_c);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string CartQuantity(CartObject c)
        {
            IProductRepository productRepository = new ProductRepository();
            List<ProductObject> listpro = productRepository.GetAllProducts().ToList();

            var _cart = GetCartsByUsername(c.Username).SingleOrDefault(p => p.ProId == c.ProId);

            var quantityInStock = listpro.FirstOrDefault(p => p.ProId == c.ProId).ProQuan;
            if (quantityInStock <= 0 || quantityInStock < c.Quantity)
            {
                return "Out of Stock!";
            }

            c.Price = _cart.Price;
            c.ProName = _cart.ProName;

            Cart _c = _mapper.Map<Cart>(c);
            using (var context = new PRNDatabaseContext())
            {
                context.Carts.Update(_c);
                context.SaveChanges();
            }
            return "Success";
        }

        public int Checkout(List<CartObject> cart, string des, double total, DeliveryAddressObject add)
        {
            if (cart.Count > 0)
            {
                IProductRepository productRepository = new ProductRepository();
                List<ProductObject> listpro = productRepository.GetAllProducts().ToList();
                foreach (var item in cart)
                {
                    var quantityInStock = listpro.FirstOrDefault(p => p.ProId == item.ProId).ProQuan;
                    var isAvail = (bool)listpro.FirstOrDefault(p => p.ProId == item.ProId).IsAvailable;

                    if (!isAvail)
                    {
                        return 2;
                    }
                    if (quantityInStock <= 0 || quantityInStock < item.Quantity)
                    {
                        return 0;
                    }
                    
                }
                string OID = "";
                GetNewOrderID(ref OID);

                OrderObject ord = new OrderObject
                {
                    OrderId = OID,
                    StaffId = null,
                    Username = cart[0].Username,
                    TotalPrice = (decimal)total,
                    StartDate = DateTime.Now,
                    EndDate = null,
                    Description = des,
                    Status = 1,
                };
                Order _ord = _mapper.Map<Order>(ord);
                using (var context = new PRNDatabaseContext())
                {
                    context.Orders.Add(_ord);
                    context.SaveChanges();
                }

                foreach (var c in cart)
                {
                    var product = listpro.FirstOrDefault(p => p.ProId == c.ProId);
                    var quantityUpdate = product.ProQuan - c.Quantity;

                    product.ProQuan = quantityUpdate;

                    Product _pro = _mapper.Map<Product>(product);
                    using (var context = new PRNDatabaseContext())
                    {
                        context.Products.Update(_pro);
                        context.SaveChanges();
                    }

                    OrderDetailObject ordDetail = new OrderDetailObject
                    {
                        OrderId = OID,
                        ProId = c.ProId,
                        ProName = c.ProName,
                        Quantity = c.Quantity,
                        Price = (decimal)(c.Price * c.Quantity)
                    };

                    OrderDetail _ordDetail = _mapper.Map<OrderDetail>(ordDetail);
                    using (var context = new PRNDatabaseContext())
                    {
                        context.OrderDetails.Add(_ordDetail);
                        context.SaveChanges();
                    }

                }

                OrderAddressObject ordAddress = new OrderAddressObject
                {
                    OrderId = OID,
                    Fullname = add.Fullname,
                    PhoneNum = add.PhoneNum,
                    AddressInformation = add.AddressInformation,
                };

                OrderAddress _ordAddress = _mapper.Map<OrderAddress>(ordAddress);
                using (var context = new PRNDatabaseContext())
                {
                    context.OrderAddresses.Add(_ordAddress);
                    context.SaveChanges();
                }


                List<Cart> _c = _mapper.Map<List<Cart>>(cart);
                using (var context = new PRNDatabaseContext())
                {
                    var cartsToRemove = context.Carts.Where(cart => cart.Username == _c[0].Username).ToList();
                    context.Carts.RemoveRange(cartsToRemove);
                    context.SaveChanges();
                }

                return 1;
            }
            return 0;
        }


        public void GetNewOrderID(ref string str)
        {
            List<OrderObject> list = new List<OrderObject>();

            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var ord = context.Orders.ToList();
                    list = _mapper.Map<List<OrderObject>>(ord);
                }

                str = list
    .OrderByDescending(o => o.OrderId)
    .Select(o => o.OrderId)
    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (str == null)
            {
                str = "OD";
                int newNumber = 1;
                string newOrderId = $"{str}{newNumber:D3}";
                str = newOrderId;
            }
            else
            {
                string numericPart = str.Substring(2);
                int currentNumber = int.Parse(numericPart);

                // Increment the number
                int newNumber = currentNumber + 1;

                // Format the new product ID
                string newOrderId = $"{str.Substring(0, 2)}{newNumber:D3}";

                str = newOrderId;
            }
        }

        public List<ImportReceiptObject> GetAllImportReceipt()
        {
            List<ImportReceiptObject> list = null;
            using (var context = new PRNDatabaseContext())
            {
                var importReceipts = context.ImportReceipts.ToList();

                // If you need to convert the Payment property from decimal to int
                foreach (var importReceipt in importReceipts)
                {
                    importReceipt.Payment = (int)importReceipt.Payment;
                    list = _mapper.Map<List<ImportReceiptObject>>(importReceipts);
                }

                return list;
            }
        }

        public List<ReceiptProductObject> GetAllReceiptProduct()
        {
            List<ReceiptProductObject> list = null;
            using (var context = new PRNDatabaseContext())
            {
                var ReceiptProducts = context.ReceiptProducts.ToList();

                // If you need to convert the Payment property from decimal to int
                foreach (var importReceipt in ReceiptProducts)
                {
                    list = _mapper.Map<List<ReceiptProductObject>>(ReceiptProducts);
                }

                return list;
            }
        }
        public void AcceptOrder(string id, int staffid)
        {
            using (var context = new PRNDatabaseContext())
            {
                var update = context.Orders.SingleOrDefault(o => o.OrderId == id);

                if (update != null)
                {
                    // Update general product details
                    update.Status = 2;
                    update.StaffId = staffid;
                }
                context.SaveChanges();
            }
        }
        public void CancelOrder(string id)
        {
            using (var context = new PRNDatabaseContext())
            {
                var update = context.Orders.SingleOrDefault(o => o.OrderId == id);

                if (update != null)
                {
                    // Update general product details
                    update.Status = 0;
                }
                context.SaveChanges();
            }
            using (var ordercontext = new PRNDatabaseContext())
            {
                foreach (var order in ordercontext.OrderDetails.Where(o => o.OrderId == id))
                {
                    using (var productcontext = new PRNDatabaseContext())
                    {
                        var product = productcontext.Products.SingleOrDefault(p => p.ProId.Equals(order.ProId));
                        product.ProQuan += order.Quantity;
                        productcontext.SaveChanges();
                    }
                }
                ordercontext.SaveChanges();
            }
        }
        public void ShippedOrder(string id)
        {
            using (var context = new PRNDatabaseContext())
            {
                var update = context.Orders.SingleOrDefault(o => o.OrderId == id);

                if (update != null)
                {
                    // Update general product details
                    update.Status = 3;
                    update.EndDate = DateTime.Now;
                }
                context.SaveChanges();
            }
        }
        public void CompletedOrder(string id)
        {
            using (var context = new PRNDatabaseContext())
            {
                var update = context.Orders.SingleOrDefault(o => o.OrderId == id);

                if (update != null)
                {
                    // Update general product details
                    update.Status = 4;
                }
                context.SaveChanges();
            }
        }

    }
}
