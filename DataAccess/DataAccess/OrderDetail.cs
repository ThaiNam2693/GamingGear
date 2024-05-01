using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class OrderDetail
{
    public string OrderId { get; set; } = null!;

    public string ProId { get; set; } = null!;

    public string? ProName { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Pro { get; set; } = null!;
}
