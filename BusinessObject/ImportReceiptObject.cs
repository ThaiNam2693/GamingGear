using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ImportReceiptObject
    {
        public int ReceiptId { get; set; }

        public DateTime? DateImport { get; set; }

        public string? PersonInCharge { get; set; }

        public decimal? Payment { get; set; }
    }
}
