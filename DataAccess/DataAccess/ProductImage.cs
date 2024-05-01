using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataAccess;

public partial class ProductImage
{
    public string ProId { get; set; } = null!;

    public string? ProductImage1 { get; set; }

    public virtual Product Pro { get; set; } = null!;
}
