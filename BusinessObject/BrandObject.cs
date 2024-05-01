using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BrandObject
    {
        public int BrandId { get; set; }

        public string? BrandName { get; set; }

        public bool? IsAvailable { get; set; }

        public string? BrandLogo { get; set; }

        public virtual ICollection<ProductObject> Products { get; set; } = new List<ProductObject>();
    }
}
