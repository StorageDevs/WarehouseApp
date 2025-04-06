using Authorization.Data;
using Authorization.Model;
using Authorization.Services.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg;

namespace Authorization.Services.IAuthService
{
    public class Auth : IAuth
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly ITokenGenerator tokenGenerator;

        public Auth(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenGenerator tokenGenerator)
        {
            _dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<object> AssignRole(string UserName, string roleName)
        {
            var user = await _dbContext.applicationUsers.FirstOrDefaultAsync(user => user.NormalizedUserName == UserName.ToUpper());

            if (user != null)
            {
                //if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                //{
                //    roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                //}
                await userManager.AddToRoleAsync(user, roleName);

                return new { result = user, message = "Successfull assingment!" };
            }
            return new { result = "", message = "Unuccessfull assingment!" };
            }

        public async Task<object> RemoveRole(string UserName, string roleName)
        {
            var user = await _dbContext.applicationUsers.FirstOrDefaultAsync(user => user.NormalizedUserName == UserName.ToUpper());

            if (user != null)
            {
                bool isInRole = await userManager.IsInRoleAsync(user, roleName);
                if (!isInRole)
                {
                    return new { result = "", message = "No such role for this user" };
                }
                await userManager.RemoveFromRoleAsync(user, roleName);

                return new { result = user, message = "Successfull unassingment!" };
            }
            return new { result = "", message = "No such user" };
        }

        public async Task<object> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _dbContext.applicationUsers.FirstOrDefaultAsync(user => user.NormalizedUserName == loginRequestDto.UserName.ToUpper());

            bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (isValid)
            {
                var roles = await userManager.GetRolesAsync(user);
                var jwtToken = tokenGenerator.GenerateToken(user, roles);

                return new { result = new { user.UserName, user.Email }, message = "Successfull login.", token = jwtToken };
            }

            return new { result = "", message = "User is not registered.", token = "" };
        }

        public async Task<object> Register(RegisterRequestDto registerRequestDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerRequestDto.UserName,
                FullName = registerRequestDto.FullName,
                Email = registerRequestDto.Email,
            };

            var result = await userManager.CreateAsync(user, registerRequestDto.Password);

            if (result.Succeeded)
            {
                var userReturn = await _dbContext.applicationUsers.FirstOrDefaultAsync(user => user.UserName == registerRequestDto.UserName);

                return new { result = userReturn, message = "Sucessfull registration." };
            }

            return new { result = "", Message = result.Errors.FirstOrDefault().Description };
        }

        public async Task<object> GetAllUser()
        {

           var users= await userManager.Users.ToListAsync();
           var userRoleList= new List<object>();

            foreach (var user in users)
            {
                var role= await userManager.GetRolesAsync(user);
                userRoleList
                .Add(new
                    {
                        userID = user.Id,
                        userName = user.UserName,
                        FullName = user.FullName,
                        password = user.PasswordHash,
                        Email = user.Email,
                        role = role
                    }
                );
                    
            }

                                        
                if (users != null)
                {
                    return new { result = userRoleList, message = "Successfull query."};
                }

                return new { result = "", message = "No user in database."};

        }

        public async Task<object> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return new { result = user, message = "Sucessfull delete." };
            }
            return new { result = "", message = "Unsucessfull delete. No such user. " };

        }

    }
}
