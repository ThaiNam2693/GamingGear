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
    public class OrderRepository : IOrderRepository
    {

        public IEnumerable<CartObject> GetCartsByUsername(string username)=>OrderDAO.Instance.GetCartsByUsername(username);

        public IEnumerable<OrderObject> GetOrderByUsername(string username)=>OrderDAO.Instance.GetOrdersByUsername(username);
        public IEnumerable<OrderDetailObject> GetOrderDetail(string id)=>OrderDAO.Instance.GetOrderDetail(id);
        public OrderAddressObject GetOrderAddress(string id)=>OrderDAO.Instance.GetOrderAddress(id);
        public IEnumerable<OrderAddressObject> GetAllOrderAddress()=>OrderDAO.Instance.GetAllOrderAddress();
        public void AddCart(CartObject c) => OrderDAO.Instance.AddCart(c);

        public void DeleteCart(string username, string proID)=> OrderDAO.Instance.DeleteCart(username, proID);

        public string CartQuantity(CartObject c) => OrderDAO.Instance.CartQuantity(c);

        public int Checkout(List<CartObject> cart, string des, double total, DeliveryAddressObject add)=> OrderDAO.Instance.Checkout(cart, des, total, add);

        public IEnumerable<OrderObject> GetAllOrders() => OrderDAO.Instance.GetAllOrder();

        public IEnumerable<OrderDetailObject> GetAllOrderDetail() => OrderDAO.Instance.GetAllOrderDetail();

        public List<ImportReceiptObject> GetAllImportReceipt() => OrderDAO.Instance.GetAllImportReceipt();

        public List<ReceiptProductObject> GetAllReceiptProduct() => OrderDAO.Instance.GetAllReceiptProduct();

        public IEnumerable<OrderObject> GetAllOrder() => OrderDAO.Instance.GetAllOrder();
        public void AcceptOrder(string id, int staffid) => OrderDAO.Instance.AcceptOrder(id, staffid);
        public void CancelOrder(string id) => OrderDAO.Instance.CancelOrder(id);
        public void ShippedOrder(string id) => OrderDAO.Instance.ShippedOrder(id);
        public void CompletedOrder(string id) => OrderDAO.Instance.CompletedOrder(id);
    }
}
