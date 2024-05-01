using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class OrderDetailObject
    {
        public string OrderId { get; set; } = null!;

        public string ProId { get; set; } = null!;

        public string? ProName { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public virtual OrderObject Order { get; set; } = null!;

        public virtual ProductObject Pro { get; set; } = null!;
    }
}
