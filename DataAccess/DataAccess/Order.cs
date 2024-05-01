using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public int? StaffId { get; set; }

    public string Username { get; set; } = null!;

    public decimal? TotalPrice { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual Manager? Staff { get; set; }

    public virtual Customer UsernameNavigation { get; set; } = null!;
}
