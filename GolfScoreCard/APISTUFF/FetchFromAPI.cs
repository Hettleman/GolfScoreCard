namespace GolfScoreCard;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

public class FetchFromAPI
{
    static async Task Main(string[] args)
    {
        using (var client = new HttpClient())
        {
            const string baseUrl = "https://api.golfcourseapi.com/v1/search";
            const string apiKey  = "3YAAYTTX6EJRBN735FGSEM4GVM";

            Console.WriteLine("Ex: oxmoor ridge, Lubbock Country Club, Pebble Beach");
            Console.Write("Enter a golf course name: ");
            var courseSelection = Console.ReadLine();

            var requestUrl = $"{baseUrl}?search_query={Uri.EscapeDataString(courseSelection)}";
            client.DefaultRequestHeaders.Add("Authorization", $"Key {apiKey}");

            try
            {
                var response = await client.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                    return;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var path = "/Users/rayhettleman/RiderProjects/GolfScoreCard/GolfScoreCard/APISTUFF/CourseDataOutput.json";
                await File.WriteAllTextAsync(path, responseBody);
                Console.WriteLine($"{courseSelection} saved to: {path}");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var courseData = JsonSerializer.Deserialize<CourseResponse>(responseBody, options);

                if (courseData?.courses?.Count > 0)
                {
                    Console.WriteLine("State:");
                    Console.WriteLine(GetLocation(courseData));

                    Console.WriteLine("Par:");
                    Console.WriteLine(GetFemalePar(courseData));
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
    }
    
    public static string GetLocation(CourseResponse courseData)
    {
        return courseData.courses[0].location.state;
    }
    
    public static int GetFemalePar(CourseResponse courseData)
    {
        return courseData.courses[0].tees.female[0].par_total;
    }
}
