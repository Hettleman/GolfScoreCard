using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolfScoreCard; // for Score & User

namespace GolfScoreCard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ScoresController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> AddScore([FromBody] Score score)
        {
            if (string.IsNullOrWhiteSpace(score.username))
                return BadRequest("Username is required.");

            var user = await _context.Users.FindAsync(score.username);
            if (user == null)
                return BadRequest("User does not exist.");

            if (string.IsNullOrWhiteSpace(score.courseName)
             || score.courseName.Length > 50)
                return BadRequest("Course name is required and must be 50 characters or fewer.");

            if (score.userScore <= 0 || score.coursePar <= 0)
                return BadRequest("User score and course par must be positive numbers.");

            if (score.dateTime == default)
                score.dateTime = DateTime.UtcNow;

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
            return Ok("Score added successfully.");
        }

        [HttpDelete("{scoreId}")]
        public async Task<IActionResult> DeleteScore(int scoreId)
        {
            var score = await _context.Scores.FindAsync(scoreId);
            if (score == null) return NotFound("Score not found.");

            _context.Scores.Remove(score);
            await _context.SaveChangesAsync();
            return Ok("Score deleted successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetScores([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required.");

            var scores = await _context.Scores
                .Where(s => s.username == username)
                .ToListAsync();

            return Ok(scores);
        }
    }
}
