using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfScoreCard.APISTUFF;
using System.Text.Json;

namespace GolfScoreCard.Pages;

public class CourseInfoModel : PageModel
{
    private readonly FetchFromAPI _api;

    public CourseInfoModel(FetchFromAPI api)
    {
        _api = api;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public string CourseName { get; set; }
    public string ClubName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int CourseId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var json = await _api.GetCourseByIdAsync(Id);
            var course = JsonDocument.Parse(json).RootElement;
            Console.WriteLine("Raw API response:");
            Console.WriteLine(json); // ✅ log it

            CourseId = course.GetProperty("id").GetInt32();
            CourseName = course.GetProperty("course_name").GetString();
            ClubName = course.GetProperty("club_name").GetString();

            if (course.TryGetProperty("location", out var location))
            {
                Address = location.GetProperty("address").GetString();
                City = location.GetProperty("city").GetString();
                State = location.GetProperty("state").GetString();
            }
            else
            {
                Address = City = State = "Unknown";
            }

            return Page();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch course: {ex.Message}");
            return NotFound();
        }
    }
}