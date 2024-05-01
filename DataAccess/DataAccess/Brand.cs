using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Brand
{
    public int BrandId { get; set; }

    public string? BrandName { get; set; }

    public bool? IsAvailable { get; set; }

    public string? BrandLogo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
