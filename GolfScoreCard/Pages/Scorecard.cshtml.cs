// Pages/Scorecard.cshtml.cs
using System;                            // for Array.Empty<>
using System.Linq;                       // for FirstOrDefault() and Select()
using System.Text.Json;                  // for JsonSerializer & JsonSerializerOptions
using Microsoft.AspNetCore.Hosting;      // for IWebHostEnvironment
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfScoreCard.APISTUFF;            // for CourseResponse, Course, Tees, Tee, Hole

namespace GolfScoreCard.Pages
{
    public class ScorecardModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public int[]  HolePars    { get; private set; }
        public string CourseName { get; private set; }

        public ScorecardModel(IWebHostEnvironment env)
            => _env = env;

        public void OnGet()
        {
            // locate CourseDataOutput.json in your project root
            var path = System.IO.Path.Combine(_env.ContentRootPath, "CourseDataOutput.json");
            if (!System.IO.File.Exists(path))
            {
                CourseName = "No course data found";
                HolePars   = Array.Empty<int>();
                return;
            }

            // read & parse the JSON
            var rawJson = System.IO.File.ReadAllText(path);
            var full    = JsonSerializer.Deserialize<CourseResponse>(
                              rawJson,
                              new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                          );

            // grab the first course
            var course = full?.Courses?.FirstOrDefault();
            if (course == null)
            {
                CourseName = "No course in JSON";
                HolePars   = Array.Empty<int>();
                return;
            }

            // display name
            CourseName = $"{course.ClubName} – {course.CourseName}";

            // pick female tee if available, otherwise male
            var teeSet = course.Tees?.Female?.FirstOrDefault()
                     ?? course.Tees?.Male?.FirstOrDefault();

            if (teeSet?.Holes == null)
            {
                HolePars = Array.Empty<int>();
                return;
            }

            // extract each hole's par in the order returned
            HolePars = teeSet.Holes
                       .Select(h => h.Par)
                       .ToArray();
        }
    }
}
