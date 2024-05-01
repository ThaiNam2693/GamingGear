using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Category
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public bool? IsAvailable { get; set; }

    public string? Keyword { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
