using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using GolfScoreCard; // for User
using GolfScoreCard.Controllers; // adjust namespace if needed
using GolfScoreCard.Observers;

namespace GolfScoreCard.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // controller class for talking to the API
    public class UsersController : ControllerBase //controller class for talking to the API
    {
        private readonly AppDbContext _context; //private field for the database context
        public UsersController(AppDbContext context) => _context = context; //constructor

        [HttpPost] // POST method for creating a new user
        public async Task<IActionResult> CreateUser([FromBody] User user) //user class for creating a new user
        {
            if (string.IsNullOrWhiteSpace(user.username) //verify username and password are not null
                || user.username.Length < 3
                || user.username.Length > 30)
                return BadRequest("Username must be between 3 and 30 characters.");

            if (string.IsNullOrWhiteSpace(user.passwordHash) //verify password length
                || user.passwordHash.Length < 6)
                return BadRequest("Password must be at least 6 characters.");

            if (string.IsNullOrWhiteSpace(user.sex) //verify sex is valid value
                || (user.sex != "male" && user.sex != "female" && user.sex != "other"))
                return BadRequest("Sex must be 'male', 'female', or 'other'.");

            if (user.handicap < 0 || user.handicap > 54) //verify handicap is valid value
                return BadRequest("Handicap must be between 0 and 54.");

            if (await _context.Users.AnyAsync(u => u.username == user.username)) //verify username is unique
                return BadRequest("Username already exists.");

            user.passwordHash = BCrypt.Net.BCrypt.HashPassword(user.passwordHash); //hash password
            _context.Users.Add(user); //add user to database
            await _context.SaveChangesAsync(); //save changes to database

            return Ok("User created successfully.");
        }

        [HttpDelete("{username}")] // DELETE method for deleting a user by username
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _context.Users.FindAsync(username); //find user by username
            if (user == null) return NotFound("User not found."); //verify user exists

            _context.Users.Remove(user); //remove user from database
            await _context.SaveChangesAsync(); //save changes to database
            return Ok("User deleted successfully.");
        }

        [HttpPost("login")] // POST method for logging in a user
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username) //verify username and password are not null
             || string.IsNullOrWhiteSpace(loginRequest.Password))
                return BadRequest("Username and password are required."); 

            var user = await _context.Users.FindAsync(loginRequest.Username);
            if (user == null
             || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.passwordHash)) //compares hashes to verify password
                return Unauthorized("Invalid username or password."); 

            return Ok("Login successful.");
        }
        
        [HttpPut("{username}/handicap")] // PUT method for updating a user's handicap'
        public async Task<IActionResult> UpdateHandicap(string username, [FromBody] decimal newHandicap)
        {
            var user = await _context.Users.FindAsync(username); //find user by username
            if (user == null) //verify user exists
                return NotFound(new { message = "User not found." });
            
            var oldHandicap = user.handicap;
            user.handicap = newHandicap;
            
            await _context.SaveChangesAsync(); //save changes to database
            
            var observer = new HandicapObserver(); //create observer instance
            observer.HandicapChange(user, oldHandicap, newHandicap); //notify observer of change
            
            return Ok(new //return updated handicap
            {
                username,
                oldHandicap,
                newHandicap,
                message = "Handicap updated successfully."
            });
        }
    }

    public class LoginRequest //login request class 
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
