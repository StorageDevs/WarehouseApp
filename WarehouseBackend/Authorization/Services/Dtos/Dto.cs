namespace Authorization.Services.Dtos
{
    public record RegisterRequestDto(string UserName, string FullName, String Password, string Email);

    public record LoginRequestDto(string UserName, string Password);

}
