using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject;
using DataAccess.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;

namespace DataAccess.DAOs
{
    public class ManagerDAO
    {
        private readonly IMapper _mapper;

        private static ManagerDAO instance;

        // Private constructor that accepts IMapper
        private ManagerDAO(IMapper mapper)
        {
            _mapper = mapper;
        }

        public static ManagerDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())));
                    instance = new ManagerDAO(mapper);
                }
                return instance;
            }
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

        //Get manager using email
        public ManagerObject GetManagerByEmail(string email)
        {
            ManagerObject manager = null;
            try
            {
                using (var context = new PRNDatabaseContext())
                {
                    var mana = context.Managers.Where(m => m.Email == email);
                    manager = _mapper.Map<ManagerObject>(mana);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return manager;
        }
        //Update staff information
        public void UpdateStaffAccount(ManagerObject manager)
        {
            using (var context = new PRNDatabaseContext())
            {
                var update = context.Managers.SingleOrDefault(u => u.Username.Equals(manager.Username));

                if (update != null)
                {
                    // Update general product details
                    update.Fullname = manager.Fullname;
                    update.PhoneNum = manager.PhoneNum;
                    update.Email = manager.Email;
                    update.Ssn = manager.Ssn;
                    update.LivingAddress = manager.LivingAddress;
                }
                context.SaveChanges();
            }

        }
    }
}
