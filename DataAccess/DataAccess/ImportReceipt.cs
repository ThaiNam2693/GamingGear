using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess;

public partial class ImportReceipt
{
    public int ReceiptId { get; set; }

    public DateTime? DateImport { get; set; }

    public string? PersonInCharge { get; set; }

    public decimal? Payment { get; set; }
}
