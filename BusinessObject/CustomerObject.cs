using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CustomerObject
    {
        public string Username { get; set; } = null!;

        public string? Password { get; set; }

        public string? Fullname { get; set; }

        public string? PhoneNum { get; set; }

        public string? Email { get; set; }

        public List<DeliveryAddressObject> addresses { get; set; } = new List<DeliveryAddressObject>();

        public virtual ICollection<OrderObject> Orders { get; set; } = new List<OrderObject>();
    }
}
