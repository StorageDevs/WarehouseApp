using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseBackend.Models;
using static WarehouseBackend.Models.User_DTO;

namespace WarehouseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public UsersController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/Users //Működik
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5 //Működik
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }



        //Működik
        // PUT: api/Users/5 
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUser updateUser)
        {
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser != null)
            {
                existingUser.UserName = updateUser.UserName;
                existingUser.Password = updateUser.Password;
                existingUser.Role = updateUser.Role;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
                return Ok();
            }


            return NotFound();

        }





        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUser createUser)
        {
            var NewUser = new User();
            { 
                NewUser.UserName = createUser.UserName;
                NewUser.Password = createUser.Password;
                NewUser.Role = createUser.Role;
                NewUser.Hash = createUser.Hash;
                NewUser.Salt = createUser.Salt;
                        
            }

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = NewUser.UserId }, NewUser);

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")] //
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


    }
}
