using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IManagerRepository
    {
        IEnumerable<ManagerObject> GetAllManagers();
        ManagerObject GetManagerByEmail(string email);
        void UpdateStaffAccount(ManagerObject manager);
    }
}
