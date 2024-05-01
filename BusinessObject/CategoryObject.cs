using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CategoryObject
    {
        public int CatId { get; set; }

        public string? CatName { get; set; }

        public bool? IsAvailable { get; set; }

        public string? Keyword { get; set; }

        public virtual ICollection<ProductObject> Products { get; set; } = new List<ProductObject>();
    }


}
