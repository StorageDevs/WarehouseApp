using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    public class TransactionDisplayItem
    {
        public int TransactionId { get; set; }
        public int MaterialNumber { get; set; }
        public string MaterialDescription { get; set; }
        public string TransferFrom { get; set; }
        public string TransferTo { get; set; }
        public int TransferedQuantity { get; set; }
        public string TransferBy { get; set; }
        public DateTime TransferDate { get; set; }
    }
}



