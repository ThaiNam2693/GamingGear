using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class OrderAddress
{
    public string OrderId { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? PhoneNum { get; set; }

    public string? AddressInformation { get; set; }

    public virtual Order Order { get; set; } = null!;
}
