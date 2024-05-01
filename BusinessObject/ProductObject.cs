using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProductObject
    {
        public string ProId { get; set; } = null!;

        public int BrandId { get; set; }

        public int CatId { get; set; }

        public string? ProName { get; set; }

        public int? ProQuan { get; set; }

        public string? ProDes { get; set; }

        public decimal? ProPrice { get; set; }

        public int? DiscountPercent { get; set; }

        public bool? IsAvailable { get; set; }

        public List<string> img { get; set; } = new List<string>();
        public Dictionary<string, string> pro_attribute { get; set; } = new Dictionary<string, string>();


        public virtual BrandObject Brand { get; set; } = null!;

        public virtual CategoryObject Cat { get; set; } = null!;
    }
}
