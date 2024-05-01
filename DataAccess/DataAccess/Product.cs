using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Product
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

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Cat { get; set; } = null!;

}
