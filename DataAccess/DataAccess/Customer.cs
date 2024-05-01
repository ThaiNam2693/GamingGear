using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Customer
{
    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? Fullname { get; set; }

    public string? PhoneNum { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
