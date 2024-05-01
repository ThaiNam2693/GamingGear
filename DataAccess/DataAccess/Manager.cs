using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Manager
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Fullname { get; set; }

    public string Ssn { get; set; } = null!;

    public string? LivingAddress { get; set; }

    public string? PhoneNum { get; set; }

    public bool? IsAdmin { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
