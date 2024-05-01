using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class ReceiptProduct
{
    public int ReceiptId { get; set; }

    public string ProId { get; set; } = null!;

    public string? ProName { get; set; }

    public int? Amount { get; set; }

    public decimal? Price { get; set; }

    public virtual Product Pro { get; set; } = null!;

    public virtual ImportReceipt Receipt { get; set; } = null!;
}
