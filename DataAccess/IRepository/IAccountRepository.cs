using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAccountRepository
    {
        CustomerObject GetCustomer(string username, string password);
        ManagerObject GetManager(string username, string password);

        CustomerObject GetCustomerByUsername(string username);
        ManagerObject GetManagerByUsername(string username);

        List<DeliveryAddressObject> GetDeliveryAddress(string username);

        bool CheckEmail(string email);


        void AddCustomer(CustomerObject customer);
        void Update(CustomerObject customer);
        void ChangePassword(CustomerObject customer, string newPass);
        void UpdateAddress(DeliveryAddressObject add);
        int AddAddress(DeliveryAddressObject add, string username);
        void DeleteAddress(int id);

        public IEnumerable<CustomerObject> GetAllCustomer();
        public IEnumerable<ManagerObject> GetAllManager();
    }
}
