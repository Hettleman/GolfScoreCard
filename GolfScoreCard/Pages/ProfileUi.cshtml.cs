using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
        }
    }
}