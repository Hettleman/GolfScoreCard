// APISTUFF/FetchFromAPI.cs
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GolfScoreCard.APISTUFF;

public class FetchFromAPI
{
    private readonly HttpClient _httpClient;

    public FetchFromAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Key 3YAAYTTX6EJRBN735FGSEM4GVM");
    }

    public async Task<string> SearchCoursesAsync(string query)
    {
        var url = $"https://api.golfcourseapi.com/v1/search?search_query={Uri.EscapeDataString(query)}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}