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
    public class AccountRepository : IAccountRepository
    {
        public void AddCustomer(CustomerObject customer)=>AccountDAO.Instance.AddCustomer(customer);
        public void Update(CustomerObject customer)=>AccountDAO.Instance.UpdateCustomer(customer);
        public void UpdateAddress(DeliveryAddressObject add)=>AccountDAO.Instance.UpdateAddress(add);
        public int AddAddress(DeliveryAddressObject add, string username)=>AccountDAO.Instance.AddAddress(add, username);
        public void DeleteAddress( int id)=>AccountDAO.Instance.DeleteAddress(id);
        public void ChangePassword(CustomerObject customer, string newPass)=>AccountDAO.Instance.ChangePassword(customer, newPass);


        public bool CheckEmail(string email)=>AccountDAO.Instance.CheckEmail(email);


        public CustomerObject GetCustomer(string username, string password)=> AccountDAO.Instance.GetCustomer(username, password);


        public CustomerObject GetCustomerByUsername(string username)=>AccountDAO.Instance.GetCustomerByUsername(username);

        public List<DeliveryAddressObject> GetDeliveryAddress(string username)=>AccountDAO.Instance.GetDeliveryAddress(username);

        public ManagerObject GetManager(string username, string password)=>AccountDAO.Instance.GetManager(username, password);


        public ManagerObject GetManagerByUsername(string username)=>AccountDAO.Instance.GetManagerByUsername(username);

        public IEnumerable<CustomerObject> GetAllCustomer() => AccountDAO.Instance.GetAllCustomers();

        public IEnumerable<ManagerObject> GetAllManager() => AccountDAO.Instance.GetAllManagers();

    }
}
