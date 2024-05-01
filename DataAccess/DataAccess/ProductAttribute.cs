using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class ProductAttribute
{
    public string ProId { get; set; } = null!;

    public string? Feature { get; set; }

    public string? Des { get; set; }

    public virtual Product Pro { get; set; } = null!;
}
