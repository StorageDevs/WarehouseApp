namespace WarehouseBackend.Models
{
    public class Material_DTO

    {   
        public record GetAllMaterialDTO(int MaterialId, int MaterialNumber, string MaterialDescription, string Unit, decimal PriceUnit);

        public record CreateMaterial(int MaterialNumber, string MaterialDescription, string Unit,decimal PriceUnit);

        public record UpdateMaterial(int MaterialId, int MaterialNumber, string MaterialDescription, string Unit, decimal PriceUnit);
    }
}
