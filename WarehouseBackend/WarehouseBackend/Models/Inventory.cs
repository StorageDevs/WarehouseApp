using System;
using System.Collections.Generic;

namespace WarehouseBackend.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int MaterialId { get; set; }

    public int LocationId { get; set; }

    public int Quantity { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;
}
