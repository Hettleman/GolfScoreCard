using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolfScoreCard;
using GolfScoreCard.Logic;

namespace GolfScoreCard.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // controller class for talking to the API
    public class ScoresController : ControllerBase
    {
        private readonly AppDbContext _context; //private field for the database context

        public ScoresController(AppDbContext context) //constructor
        {
            _context = context;
        }

        [HttpPost] // POST method for adding a new score
        public async Task<IActionResult> AddScore([FromBody] Score score)
        {
            if (string.IsNullOrWhiteSpace(score.username)) //verify username and password are not null
                return BadRequest("Username is required.");

            var user = await _context.Users.FindAsync(score.username); //find user by username
            if (user == null)
                return BadRequest("User does not exist.");

            if (string.IsNullOrWhiteSpace(score.courseName) || score.courseName.Length > 50) //verify course name is valid value
                return BadRequest("Course name is required and must be 50 characters or fewer.");

            if (score.userScore <= 0 || score.coursePar <= 0) //verify score and course par are valid values
                return BadRequest("User score and course par must be positive numbers.");

            if (score.dateTime == default) //verify date time is valid value
                score.dateTime = DateTime.UtcNow;

            _context.Scores.Add(score); //add score to database
            await _context.SaveChangesAsync(); //save changes to database

            double courseRating = Request.Headers.TryGetValue("CourseRating", out var ratingVal) && double.TryParse(ratingVal, out var rVal)
                ? rVal : 0; 

            int courseSlope = Request.Headers.TryGetValue("CourseSlope", out var slopeVal) && int.TryParse(slopeVal, out var sVal)
                ? sVal : 0;

            if (courseRating > 0 && courseSlope > 0) //update handicap if course rating and slope are provided
            {
                var allScores = await _context.Scores
                    .Where(s => s.username == score.username)
                    .ToListAsync();

                var calculator = HandicapCalculator.Instance; //create handicap calculator instance

                var differentials = allScores 
                    .Select(s => calculator.CalculateDifferential(s.userScore, courseRating, courseSlope))
                    .OrderBy(d => d)
                    .Take(5)
                    .ToList();

                if (differentials.Count > 0) //update handicap if there are at least 5 scores
                {
                    user.handicap = calculator.CalculateAverage(differentials); //calculate average differential
                    await _context.SaveChangesAsync();  //save changes to database
                }
            }

            return Ok("Score added and handicap updated."); 
        }

        [HttpDelete("{scoreId}")] // DELETE method for deleting a score by ID
        public async Task<IActionResult> DeleteScore(int scoreId)
        {
            var score = await _context.Scores.FindAsync(scoreId); //find score by ID
            if (score == null) //verify score exists
                return NotFound("Score not found.");

            _context.Scores.Remove(score); //remove score from database
            await _context.SaveChangesAsync(); //save changes to database
            return Ok("Score deleted successfully."); 
        }

        [HttpGet] // GET method for getting all scores
        public async Task<IActionResult> GetScores([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username)) //verify username is not null
                return BadRequest("Username is required.");

            var scores = await _context.Scores
                .Where(s => s.username == username)
                .OrderByDescending(s => s.dateTime)
                .ToListAsync(); //get all scores for user

            return Ok(scores); //return scores
        }
    }
}
