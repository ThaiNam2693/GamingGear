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
    public class ManagerRepository: IManagerRepository
    {
        public IEnumerable<ManagerObject> GetAllManagers() => ManagerDAO.Instance.GetAllManagers();
        public ManagerObject GetManagerByEmail(string email) => ManagerDAO.Instance.GetManagerByEmail(email);
        public void UpdateStaffAccount(ManagerObject manager) => ManagerDAO.Instance.UpdateStaffAccount(manager);
    }
}
