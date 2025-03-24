using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public virtual Material Material { get; set; } = null!;
    [JsonIgnore]
    public virtual Location TransactionFrom { get; set; } = null!;
    [JsonIgnore]
    public virtual Location TransactionTo { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
