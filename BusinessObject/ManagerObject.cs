using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ManagerObject
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Fullname { get; set; }

        public string Ssn { get; set; } = null!;

        public string? LivingAddress { get; set; }

        public string? PhoneNum { get; set; }

        public bool? IsAdmin { get; set; }

        public virtual ICollection<OrderObject> Orders { get; set; } = new List<OrderObject>();
    }


}
