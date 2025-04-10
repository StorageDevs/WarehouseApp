using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    public class InventoryDisplayItem
    {
        public int MaterialId { get; set; }
        public int MaterialNumber { get; set; }
        public string MaterialDescription { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int Quantity { get; set; }
    }
}

