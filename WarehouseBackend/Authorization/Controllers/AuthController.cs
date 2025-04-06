using Authorization.Model;
using Authorization.Services.Dtos;
using Authorization.Services.IAuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;

        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Register")]
        public async Task<ActionResult> AddNewUser(RegisterRequestDto registerRequestDto)
        {
            var user = await auth.Register(registerRequestDto);

            if (user != null)
            {
                return StatusCode(201, user);
            }

            return BadRequest(new { result="",message="Registration failed." });
        }

        
        [HttpPost("Login")]

        public async Task<ActionResult> LoginUser(LoginRequestDto loginRequestDto)

        {
            var res = await auth.Login(loginRequestDto);
            if (res != null)
            {
                return StatusCode(200, res); 
            }
            return NotFound(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Assignrole")]

        public async Task<ActionResult> AddRole(string UserName, string roleName)
        {
            var res=await auth.AssignRole(UserName, roleName);

            if (res != null)
            {
                return Ok(res);
            }

            return BadRequest(res);

        }

        [Authorize(Roles = "admin")]
        [HttpPost("Removerole")]

        public async Task<ActionResult> RemoveRole(string UserName, string roleName)
        {
            var res = await auth.RemoveRole(UserName, roleName);

            if (res != null)
            {
                return Ok(res);
            }

            return BadRequest(res);

        }

        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var res = await auth.DeleteUser(id);
            try
            {
                return Ok(res);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpGet("GetAllUser")]
        public async Task<ActionResult> GetUsers()
        {

            var res=await auth.GetAllUser();
            try
            {
                return Ok(res);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("ModifyUser")]
        //public async Task<ActionResult> ModifyUserer()
        //{
        //    try
        //    {

        //        return Ok();
        //    }

        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
} 

