using System;
using System.Collections.Generic;

namespace WarehouseBackend.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = null!;

    public string LocationDescription { get; set; } = null!;

    public int LocationCapacity { get; set; }

    [JsonIgnore]

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    [JsonIgnore]

    public virtual ICollection<Transaction> TransactionTransactionFroms { get; set; } = new List<Transaction>();

    [JsonIgnore]
    public virtual ICollection<Transaction> TransactionTransactionTos { get; set; } = new List<Transaction>();
}
