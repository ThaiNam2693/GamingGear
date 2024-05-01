using AutoMapper;
using BusinessObject;
using DataAccess.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class AccountDAO
    {
        private readonly IMapper _mapper;

        private static AccountDAO instance;

        // Private constructor that accepts IMapper
        private AccountDAO(IMapper mapper)
        {
            _mapper = mapper;
        }

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())));
                    instance = new AccountDAO(mapper);
                }
                return instance;
            }
        }

        public CustomerObject GetCustomer(string username,  string password)
        {
            CustomerObject cus = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var _cus = context.Customers.SingleOrDefault(cus => cus.Username == username && cus.Password== CalculateMD5Hash(password));
                    cus = _mapper.Map<CustomerObject>(_cus);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return cus;
        }

        public IEnumerable<ManagerObject> GetAllManagers()
        {
            List<ManagerObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var managers = context.Managers.ToList();
                    list = _mapper.Map<List<ManagerObject>>(managers);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public IEnumerable<CustomerObject> GetAllCustomers()
        {
            List<CustomerObject> list = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var managers = context.Customers.ToList();
                    list = _mapper.Map<List<CustomerObject>>(managers);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }
        public ManagerObject GetManager(string username, string password)
        {
            List<ManagerObject> list = GetAllManagers().ToList();
            ManagerObject man = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    ManagerObject _man = list.FirstOrDefault(m => m.Username.Equals(username) && m.Password.Equals(CalculateMD5Hash(password)));
                    man = _mapper.Map<ManagerObject>(_man);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return man;
        }

        public string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public CustomerObject GetCustomerByUsername(string username)
        {
            CustomerObject cus = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var _cus = context.Customers.SingleOrDefault(cus => cus.Username == username);
                    cus = _mapper.Map<CustomerObject>(_cus);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return cus;
        }

        public ManagerObject GetManagerByUsername(string username)
        {
            ManagerObject man = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var _man = context.Managers.SingleOrDefault(m => m.Username == username);
                    man = _mapper.Map<ManagerObject>(_man);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return man;
        }

        public List<DeliveryAddressObject> GetDeliveryAddress(string username)
        {
            List<DeliveryAddressObject> adr = new List<DeliveryAddressObject>();
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var _adr = context.DeliveryAddresses.Where(a => a.Username == username).ToList();
                    adr = _mapper.Map<List<DeliveryAddressObject>>(_adr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return adr;
        }

        public bool CheckEmail(string email)
        {
            CustomerObject cus = null;
            ManagerObject man = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var _cus = context.Customers.SingleOrDefault(a => a.Email == email);
                    var _man = context.Managers.SingleOrDefault(a => a.Email == email);
                    cus = _mapper.Map<CustomerObject>(_cus);
                    man = _mapper.Map<ManagerObject>(_man);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return cus!=null || man!=null; //true
        }

        public void AddCustomer(CustomerObject _cus)
        {
            _cus.Password = CalculateMD5Hash(_cus.Password);
            Customer cus = _mapper.Map<Customer>(_cus);
            try
            {
                using (var context = new PRNDatabaseContext())
                {

                    context.Customers.Add(cus);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCustomer(CustomerObject _cus)
        {
            Customer cus = _mapper.Map<Customer>(_cus);
            try
            {
                using (var context = new PRNDatabaseContext())
                {

                    context.Customers.Update(cus);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DeliveryAddressObject GetAddressByID(int id)
        {
            DeliveryAddressObject add = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var _add = context.DeliveryAddresses.SingleOrDefault(c => c.ID == id);
                    add = _mapper.Map<DeliveryAddressObject>(_add);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return add;
        }

        public int AddAddress(DeliveryAddressObject _add, string username)
        {
            DeliveryAddress add = _mapper.Map<DeliveryAddress>(_add);
            add.Username = username;
            try
            {
                using (var context = new PRNDatabaseContext())
                {

                    context.DeliveryAddresses.Add(add);
                    context.SaveChanges();
                }


                using (var context = new PRNDatabaseContext())
                {
                    var deliveryAddress = context.DeliveryAddresses
                        .Where(da => da.Username == username)
                        .OrderByDescending(da => da.ID)
                        .Select(da => da.ID)
                        .FirstOrDefault();

                    if (deliveryAddress != null)
                    {
                        return  (int)deliveryAddress;
                        // Sử dụng giá trị kq tại đây.
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public void UpdateAddress(DeliveryAddressObject _add)
        {
            DeliveryAddress add = _mapper.Map<DeliveryAddress>(_add);
            try
            {
                using (var context = new PRNDatabaseContext())
                {

                    context.DeliveryAddresses.Update(add);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteAddress(int id)
        {
            DeliveryAddress add = _mapper.Map<DeliveryAddress>(GetAddressByID(id));
            try
            {
                using (var context = new PRNDatabaseContext()) 
                {
                                                                    
                    context.DeliveryAddresses.Remove(add);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void ChangePassword(CustomerObject _cus, string newpass)
        {
            _cus.Password = CalculateMD5Hash(newpass);

            Customer cus = _mapper.Map<Customer>(_cus);
            try
            {
                using (var context = new PRNDatabaseContext())
                {

                    context.Customers.Update(cus);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
