namespace GolfScoreCard;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

class Program
{
    static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            string baseUrl = "https://api.golfcourseapi.com/v1/search";
            string apiKey = "3YAAYTTX6EJRBN735FGSEM4GVM";  // Replace with your actual API key
            
            Console.WriteLine("Ex: oxmoor ridge, Lubbock Country Club, Pebble Beach");
            Console.WriteLine("Enter a golf course name: ");
            string searchQuery = Console.ReadLine();
            
            string requestUrl = $"{baseUrl}?search_query={Uri.EscapeDataString(searchQuery)}";
            client.DefaultRequestHeaders.Add("Authorization", $"Key {apiKey}");

            try
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    // Always print where files will be saved
                    Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);

                    // Force save raw API response for debugging
                    string rawPath = "/Users/rayhettleman/RiderProjects/GolfScoreCard/GolfScoreCard/CourseDataOutput.json";
                    await File.WriteAllTextAsync(rawPath, responseBody);
                    Console.WriteLine($"Raw API response saved to: {rawPath}");

                    // Try deserializing
                    CourseResponse courseData = JsonSerializer.Deserialize<CourseResponse>(responseBody, options);

                    if (courseData != null)
                    {
                        Console.WriteLine("State:");
                        Console.WriteLine(courseData.courses[0].location.state);
                        Console.WriteLine("Par:");
                        Console.WriteLine(courseData.courses[0].tees.female[0].par_total);

                    }
                    else
                    {
                        Console.WriteLine("Deserialization failed — check TestRawOutput.json for raw data.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
            }
        }
    }
}
