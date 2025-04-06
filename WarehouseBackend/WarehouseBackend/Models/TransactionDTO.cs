using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WarehouseBackend.Models
{
    
    public class TransactionDTO
    {
        public record GetAllTransaction
        {
            public int TransactionId { get; set; }
            public int MaterialNumber { get; set; }
            public string MaterialDescription { get; set; }
            public string TransferFrom { get; set; }
            public string TransferTo { get; set; }
            public int TransferedQuantity { get; set; }

            //public string TransferBy { get; set; }
            public DateTime TransferDate { get; set; }
        }

        public record CreateTransaction
        (
         int MaterialNumber,
         string TransactionFromLocationName,
         string TransactionToLocationName,
         int TransactedQty
         //string UserName
        );


    }




}
