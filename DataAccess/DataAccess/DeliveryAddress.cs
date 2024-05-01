using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class DeliveryAddress
{
    public string Username { get; set; } = null!;

    public string? AddressInformation { get; set; }

    public string? Fullname { get; set; }

    public string? PhoneNum { get; set; }

    public int ID { get; set; }
    public virtual Customer UsernameNavigation { get; set; } = null!;
}
