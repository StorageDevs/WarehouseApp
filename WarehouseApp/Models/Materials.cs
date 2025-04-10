using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    public class Material
    {
        public int MaterialId { get; set; }
        public int MaterialNumber { get; set; }
        public string MaterialDescription { get; set; }
        public string Unit { get; set; }
        public decimal PriceUnit { get; set; }
    }
}
