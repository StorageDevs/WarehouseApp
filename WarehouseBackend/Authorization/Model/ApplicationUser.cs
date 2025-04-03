using Microsoft.AspNetCore.Identity;

namespace Authorization.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName {  get; set; }


    }
}
