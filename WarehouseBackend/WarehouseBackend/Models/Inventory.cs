using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarehouseBackend.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int MaterialId { get; set; }

    public int LocationId { get; set; }

    public int Quantity { get; set; }
    
    [JsonIgnore]
    public virtual Location Location { get; set; } = null!;
    [JsonIgnore]
    public virtual Material Material { get; set; } = null!;
}
