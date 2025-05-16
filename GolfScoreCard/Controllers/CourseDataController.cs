using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GolfScoreCard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseDataOutputController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public CourseDataOutputController(IWebHostEnvironment env) => _env = env;

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            // 1. Read the raw JSON payload
            using var reader = new StreamReader(Request.Body);
            var rawJson = await reader.ReadToEndAsync();

            // 2. Parse and re-serialize with indentation
            using var doc = JsonDocument.Parse(rawJson);
            var prettyJson = JsonSerializer.Serialize(
                doc.RootElement,
                new JsonSerializerOptions { WriteIndented = true }
            );

            // 3. Write the pretty JSON to disk
            var filePath = Path.Combine(_env.ContentRootPath, "CourseDataOutput.json");
            await System.IO.File.WriteAllTextAsync(filePath, prettyJson);

            return Ok(new { written = true });
        }
    }
}