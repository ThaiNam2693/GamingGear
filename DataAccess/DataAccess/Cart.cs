using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Cart
{
    public string Username { get; set; } = null!;

    public string ProId { get; set; } = null!;

    public string? ProName { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Product Pro { get; set; } = null!;

    public virtual Customer UsernameNavigation { get; set; } = null!;
}
