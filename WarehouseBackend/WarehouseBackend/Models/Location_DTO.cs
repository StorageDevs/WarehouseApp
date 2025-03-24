namespace WarehouseBackend.Models
{
    public class Location_DTO
    {
        public record CreateLocationDTO(string LocationName, string LocationDescription, int LocationCapacity);
        public record UpdateLocationDTO(int LocationID,string LocationName, string LocationDescription, int LocationCapacity);

    }
}
