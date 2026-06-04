using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public class LoginDto
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            if (!HashHelper.VerifyPasswordHash(dto.Password, user.PasswordHash, user.Salt))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = TokenHelper.GenerateJwtToken(user, _config);
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.FullName
                }
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users
                .Select(u => new { u.Id, u.Username, u.FullName })
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        public class UpdateProfileDto
        {
            public string FullName { get; set; } = string.Empty;
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound(new { message = "User not found" });

            user.FullName = dto.FullName;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully", user = new { user.Id, user.Username, user.FullName } });
        }

        public class ChangePasswordDto
        {
            public string CurrentPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound(new { message = "User not found" });

            if (!HashHelper.VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash, user.Salt))
            {
                return BadRequest(new { message = "Incorrect current password" });
            }

            var (hash, salt) = HashHelper.CreatePasswordHash(dto.NewPassword);
            user.PasswordHash = hash;
            user.Salt = salt;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully" });
        }
    }
}
