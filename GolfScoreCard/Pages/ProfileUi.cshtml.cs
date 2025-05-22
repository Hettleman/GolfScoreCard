using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GolfScoreCard.Strategy;

namespace GolfScoreCard.Pages
{
    public class ProfileUIModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProfileUIModel(AppDbContext context)
        {
            _context = context;
        }

        public string Username { get; set; }
        public double Handicap { get; set; }
        public double RawHandicap { get; set; }  // 👈 NEW
        public List<Score> PastScores { get; set; }

        public async Task OnGetAsync()
        {
            Username = HttpContext.Session.GetString("username") ?? "Guest";

            var user = await _context.Users.FindAsync(Username);
            Handicap = (double)(user?.handicap ?? 0);

            PastScores = await _context.Scores
                .Where(s => s.username == Username)
                .OrderByDescending(s => s.dateTime)
                .ToListAsync();

            // Strategy-based Raw Handicap Calculation
            var scores = PastScores.Select(s => (double)s.userScore).ToList();
            var slopes = Enumerable.Repeat(113.0, scores.Count).ToList(); // Replace with real slope if available
            var ratings = PastScores.Select(s => (double)s.coursePar).ToList(); // Replace with real rating if stored

            var strategy = new HandicapContext(new RawHandicapStrategy());
            RawHandicap = scores.Count > 0 ? strategy.Calculate(scores, slopes, ratings) : 0;
        }
    }
}