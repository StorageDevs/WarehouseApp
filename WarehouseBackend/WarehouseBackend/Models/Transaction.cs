using System;
using System.Collections.Generic;

namespace WarehouseBackend.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public int MaterialId { get; set; }

    public int TransactionFromId { get; set; }

    public int TransactedQty { get; set; }

    public int TransactionToId { get; set; }

    public int UserId { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Location TransactionFrom { get; set; } = null!;

    public virtual Location TransactionTo { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
