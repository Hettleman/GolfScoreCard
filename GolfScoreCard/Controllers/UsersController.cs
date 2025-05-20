using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using GolfScoreCard; // for User
using GolfScoreCard.Controllers; // adjust namespace if needed

namespace GolfScoreCard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.username)
                || user.username.Length < 3
                || user.username.Length > 30)
                return BadRequest("Username must be between 3 and 30 characters.");

            if (string.IsNullOrWhiteSpace(user.passwordHash)
                || user.passwordHash.Length < 6)
                return BadRequest("Password must be at least 6 characters.");

            if (string.IsNullOrWhiteSpace(user.sex)
                || (user.sex != "male" && user.sex != "female" && user.sex != "other"))
                return BadRequest("Sex must be 'male', 'female', or 'other'.");

            if (user.handicap < 0 || user.handicap > 54)
                return BadRequest("Handicap must be between 0 and 54.");

            if (await _context.Users.AnyAsync(u => u.username == user.username))
                return BadRequest("Username already exists.");

            user.passwordHash = BCrypt.Net.BCrypt.HashPassword(user.passwordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User created successfully.");
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null) return NotFound("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User deleted successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username)
             || string.IsNullOrWhiteSpace(loginRequest.Password))
                return BadRequest("Username and password are required.");

            var user = await _context.Users.FindAsync(loginRequest.Username);
            if (user == null
             || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.passwordHash))
                return Unauthorized("Invalid username or password.");

            return Ok("Login successful.");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
