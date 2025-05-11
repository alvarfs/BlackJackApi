using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlackJackApi.Models;

namespace BlackJackApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // Lista de usuarios ordenados por dinero total
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var userList = await _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Cash = u.Cash
                }).ToListAsync();
            
            userList = userList.OrderByDescending(u => u.Cash).ToList();

            return userList;
        }

        // Recoger datos de un usuario especifico via id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var userDto = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Cash = user.Cash
            };

            return userDto;
        }

        // Actualiza la cuenta del usuario
        [HttpPut("{id}/{cash}")]
        public async Task<IActionResult> PutUser(long id, int cash)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Cash = cash;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Crear nuevo usuario, el cual siempre empieza con 100$
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            user.Cash = 100;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Cash = user.Cash
            };

            // return CreatedAtAction("GetUser", new { id = user.Id }, user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
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

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
