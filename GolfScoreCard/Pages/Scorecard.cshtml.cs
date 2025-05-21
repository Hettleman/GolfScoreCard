// Pages/Scorecard.cshtml.cs
using System;                            // for Array.Empty<>
using System.Linq;                       // for FirstOrDefault() and Select()
using System.Text.Json;                 // for JsonSerializer & JsonSerializerOptions
using Microsoft.AspNetCore.Hosting;     // for IWebHostEnvironment
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfScoreCard.APISTUFF;
using Microsoft.AspNetCore.Mvc; // for CourseResponse, Course, Tees, Tee, Hole

namespace GolfScoreCard.Pages
{
    public class ScorecardModel : PageModel
    {

        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _db;

        public int[]  HolePars       { get; private set; }
        public int[]  HoleDistances { get; private set; }
        [BindProperty] public string CourseName { get; set; }
        [BindProperty] public int CoursePar { get; set; }
        [BindProperty] public int TotalScore { get; set; }



        public ScorecardModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db; 
            _env = env;
        }
                

        public void OnGet()
        {
            var path = System.IO.Path.Combine(_env.ContentRootPath, "CourseDataOutput.json");
            if (!System.IO.File.Exists(path))
            {
                CourseName     = "No course data found";
                HolePars       = Array.Empty<int>();
                HoleDistances = Array.Empty<int>();
                return;
            }

            var rawJson = System.IO.File.ReadAllText(path);
            var full    = JsonSerializer.Deserialize<CourseResponse>(
                rawJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var course = full?.Courses?.FirstOrDefault();
            if (course == null)
            {
                CourseName     = "No course in JSON";
                HolePars       = Array.Empty<int>();
                HoleDistances = Array.Empty<int>();
                return;
            }

            var combinedName = course.ClubName + " - " + course.CourseName;
            CourseName = combinedName.Length > 50 ? combinedName.Substring(0, 50) : combinedName;

            // 1) default to male tees, 2) fallback to female
            var defaultTee = course.Tees?.Male?.FirstOrDefault()
                             ?? course.Tees?.Female?.FirstOrDefault();

            if (defaultTee?.Holes == null)
            {
                HolePars       = Array.Empty<int>();
                HoleDistances = Array.Empty<int>();
                return;
            }

            // pars from the chosen default
            HolePars = defaultTee.Holes.Select(h => h.Par).ToArray();

            // blue from male first, then female, then default
            var blueTee = course.Tees?.Male?
                              .FirstOrDefault(t => 
                                  t.TeeName.Equals("Blue", StringComparison.OrdinalIgnoreCase))
                          ?? course.Tees?.Female?
                              .FirstOrDefault(t => 
                                  t.TeeName.Equals("Blue", StringComparison.OrdinalIgnoreCase))
                          ?? defaultTee;

            HoleDistances = blueTee.Holes.Select(h => h.Yardage).ToArray();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var username = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/LoginUI");

            var scoreEntry = new Score
            {
                username = username,
                courseName = CourseName,
                userScore = TotalScore,
                coursePar = CoursePar,
                dateTime = DateTime.UtcNow
            };

            _db.Scores.Add(scoreEntry);
            await _db.SaveChangesAsync();

            return RedirectToPage("/ProfileUI"); // or thank-you page
        }


    }
}
