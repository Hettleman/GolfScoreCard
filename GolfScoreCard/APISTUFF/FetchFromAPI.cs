namespace GolfScoreCard;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

public class FetchFromAPI
{
    static async Task Fetch()
    {
        using var client = new HttpClient();
        const string baseUrl = "https://api.golfcourseapi.com/v1/search";
        const string apiKey  = "3YAAYTTX6EJRBN735FGSEM4GVM";

        Console.Write("Enter a golf course name: ");
        var courseSelection = Console.ReadLine();

        var requestUrl = $"{baseUrl}?search_query={Uri.EscapeDataString(courseSelection)}";
        client.DefaultRequestHeaders.Add("Authorization", $"Key {apiKey}");

        try
        {
            var response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var path = "/path/to/CourseDataOutput.json";
            await File.WriteAllTextAsync(path, responseBody);
            Console.WriteLine($"{courseSelection} saved to: {path}");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var courseData = JsonSerializer.Deserialize<CourseResponse>(responseBody, options);

            if (courseData?.courses?.Count > 0)
            {
                var firstCourse = courseData.courses[0];

                Console.WriteLine("State:");
                Console.WriteLine(GetLocation(firstCourse));

                Console.WriteLine("Par:");
                Console.WriteLine(GetFemalePar(firstCourse));
            }
            else
            {
                Console.WriteLine("No course data returned.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");
        }
    }
    
    // now takes a Course
    public static string GetLocation(Course course)
    {
        return course.location.state;
    }
    
    // now takes a Course
    public static int GetFemalePar(Course course)
    {
        return course.tees.female[0].par_total;
    }
}
