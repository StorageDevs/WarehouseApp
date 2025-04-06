using Authorization.Services.Dtos;
using Microsoft.AspNetCore.Identity.Data;

namespace Authorization.Services.IAuthService
{
    public interface IAuth
    {
        Task<object> Register(RegisterRequestDto registerRequestDto);

        Task<object> Login(LoginRequestDto loginRequestDto);
        Task<object> AssignRole(string Username, string roleName);
        Task<object> RemoveRole(string Username, string roleName);

        Task<object> GetAllUser();
        Task<object> DeleteUser(string id);
        //Task<object> ModifyUser();

    }
}
