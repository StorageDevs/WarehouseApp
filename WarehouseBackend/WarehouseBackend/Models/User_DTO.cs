namespace WarehouseBackend.Models
{
    public class User_DTO
    {
        public record CreateUser(string UserName, string Password, string Salt, string Hash, int Role);

        public record UpdateUser(string UserName, string Password, string Salt, string Hash, int Role);

    }
}
