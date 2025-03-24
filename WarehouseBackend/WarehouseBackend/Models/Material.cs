using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarehouseBackend.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public int MaterialNumber { get; set; }

    public string MaterialDescription { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal PriceUnit { get; set; }

    [JsonIgnore]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [JsonIgnore]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
