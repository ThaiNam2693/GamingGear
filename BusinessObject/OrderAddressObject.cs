using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class OrderAddressObject
    {
        public string OrderId { get; set; } = null!;

        public string? Fullname { get; set; }

        public string? PhoneNum { get; set; }

        public string? AddressInformation { get; set; }

        public virtual OrderObject Order { get; set; } = null!;
    }
}
