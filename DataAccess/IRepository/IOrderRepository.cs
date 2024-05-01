using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IOrderRepository
    {
        IEnumerable<CartObject> GetCartsByUsername(string username);
        IEnumerable<OrderObject> GetOrderByUsername(string username);
        IEnumerable<OrderDetailObject> GetOrderDetail(string id);
        OrderAddressObject GetOrderAddress(string id);
        IEnumerable<OrderAddressObject> GetAllOrderAddress();
        void AddCart(CartObject c);
        void DeleteCart(string username, string proID);
        string CartQuantity(CartObject c);
        int Checkout(List<CartObject> cart, string des, double total, DeliveryAddressObject add);

        public IEnumerable<OrderObject> GetAllOrders();
        public IEnumerable<OrderDetailObject> GetAllOrderDetail();

        public List<ImportReceiptObject> GetAllImportReceipt();
        public List<ReceiptProductObject> GetAllReceiptProduct();
        public IEnumerable<OrderObject> GetAllOrder();
        public void AcceptOrder(string id, int staffid);
        public void CancelOrder(string id);
        public void ShippedOrder(string id);
        public void CompletedOrder(string id);
    }
}
