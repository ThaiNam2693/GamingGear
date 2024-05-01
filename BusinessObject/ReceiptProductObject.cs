using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ReceiptProductObject
    {
        public int ReceiptId { get; set; }

        public string ProId { get; set; } = null!;

        public string? ProName { get; set; }

        public int? Amount { get; set; }

        public decimal? Price { get; set; }

        public virtual ProductObject Pro { get; set; } = null!;

        public virtual ImportReceiptObject Receipt { get; set; } = null!;
    }
}
