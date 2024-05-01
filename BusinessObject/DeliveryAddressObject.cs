using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DeliveryAddressObject
    {
        public string Username { get; set; } = null!;

        public string? AddressInformation { get; set; }

        public string? Fullname { get; set; }

        public string? PhoneNum { get; set; }
        public int ID { get; set; }

        public virtual CustomerObject UsernameNavigation { get; set; } = null!;
    }
}
