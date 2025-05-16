using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GolfScoreCard.APISTUFF
{
    public class CourseResponse
    {
        [JsonPropertyName("courses")]
        public List<Course> Courses { get; set; } = new();
    }
}