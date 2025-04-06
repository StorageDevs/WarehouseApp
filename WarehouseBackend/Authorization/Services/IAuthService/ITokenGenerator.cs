using Authorization.Model;

namespace Authorization.Services.IAuthService
{
    public interface ITokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> role);
    }
}
