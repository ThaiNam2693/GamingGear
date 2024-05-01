using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProductImageObject
    {
        public string ProId { get; set; } = null!;

        public string? ProductImage1 { get; set; }

        public virtual ProductObject Pro { get; set; } = null!;
    }
}
