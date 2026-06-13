using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(u => u.Username.ToLower().Contains(lowerSearch) ||
                                         u.FullName.ToLower().Contains(lowerSearch));
            }

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new { u.Id, u.Username, u.FullName, u.IsLocked })
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = users
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users
                .Select(u => new { u.Id, u.Username, u.FullName, u.IsLocked })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        public class CreateUserDto
        {
            public string Username { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.FullName))
            {
                return BadRequest(new { message = "Username, full name and password are required." });
            }

            var normalizedUsername = dto.Username.Trim().ToLower();
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == normalizedUsername))
            {
                return BadRequest(new { message = $"Username '{dto.Username}' is already taken." });
            }

            var (hash, salt) = HashHelper.CreatePasswordHash(dto.Password);
            var user = new User
            {
                Username = dto.Username.Trim(),
                FullName = dto.FullName.Trim(),
                PasswordHash = hash,
                Salt = salt,
                IsLocked = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new { user.Id, user.Username, user.FullName, user.IsLocked });
        }

        public class UpdateUserDto
        {
            public string Username { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string? Password { get; set; }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                return BadRequest(new { message = "Username is required." });
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                return BadRequest(new { message = "Full name is required." });
            }

            var normalizedUsername = dto.Username.Trim().ToLower();
            if (user.Username.ToLower() != normalizedUsername && await _context.Users.AnyAsync(u => u.Username.ToLower() == normalizedUsername))
            {
                return BadRequest(new { message = $"Username '{dto.Username}' is already taken." });
            }

            user.Username = dto.Username.Trim();
            user.FullName = dto.FullName.Trim();

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var (hash, salt) = HashHelper.CreatePasswordHash(dto.Password);
                user.PasswordHash = hash;
                user.Salt = salt;
            }

            await _context.SaveChangesAsync();
            return Ok(new { user.Id, user.Username, user.FullName, user.IsLocked });
        }

        [HttpPut("{id}/lock")]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            // Safety guard: cannot lock oneself
            var currentUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdStr != null && int.TryParse(currentUserIdStr, out int currentUserId) && currentUserId == id)
            {
                return BadRequest(new { message = "You cannot lock your own account." });
            }

            user.IsLocked = !user.IsLocked;
            await _context.SaveChangesAsync();

            return Ok(new { message = user.IsLocked ? "User locked successfully" : "User unlocked successfully", isLocked = user.IsLocked });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            // Safety guard: cannot delete oneself
            var currentUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdStr != null && int.TryParse(currentUserIdStr, out int currentUserId) && currentUserId == id)
            {
                return BadRequest(new { message = "You cannot delete your own account." });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
