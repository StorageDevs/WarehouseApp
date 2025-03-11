using System;
using System.Collections.Generic;

namespace WarehouseBackend.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public int MaterialNumber { get; set; }

    public string MaterialDescription { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal PriceUnit { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
