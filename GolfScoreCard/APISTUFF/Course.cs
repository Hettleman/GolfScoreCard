using System.Text.Json.Serialization;

namespace GolfScoreCard.APISTUFF
{
    public class Course
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("club_name")]
        public string ClubName { get; set; }

        [JsonPropertyName("course_name")]
        public string CourseName { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("tees")]
        public Tees Tees { get; set; }
    }
}