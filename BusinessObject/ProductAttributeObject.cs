using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProductAttributeObject
    {
        public string ProId { get; set; } = null!;

        public string? Feature { get; set; }

        public string? Des { get; set; }

        public virtual ProductObject Pro { get; set; } = null!;
    }
}
