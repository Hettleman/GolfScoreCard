using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GolfScoreCard.APISTUFF
{
    public class Tee
    {
        [JsonPropertyName("tee_name")]
        public string TeeName { get; set; }

        [JsonPropertyName("course_rating")]
        public double CourseRating { get; set; }

        [JsonPropertyName("slope_rating")]
        public int SlopeRating { get; set; }

        [JsonPropertyName("bogey_rating")]
        public double BogeyRating { get; set; }

        [JsonPropertyName("total_yards")]
        public int TotalYards { get; set; }

        [JsonPropertyName("total_meters")]
        public int TotalMeters { get; set; }

        [JsonPropertyName("number_of_holes")]
        public int NumberOfHoles { get; set; }

        [JsonPropertyName("par_total")]
        public int ParTotal { get; set; }

        [JsonPropertyName("front_course_rating")]
        public double FrontCourseRating { get; set; }

        [JsonPropertyName("front_slope_rating")]
        public int FrontSlopeRating { get; set; }

        [JsonPropertyName("front_bogey_rating")]
        public double FrontBogeyRating { get; set; }

        [JsonPropertyName("back_course_rating")]
        public double BackCourseRating { get; set; }

        [JsonPropertyName("back_slope_rating")]
        public int BackSlopeRating { get; set; }

        [JsonPropertyName("back_bogey_rating")]
        public double BackBogeyRating { get; set; }

        // ← This was missing before
        [JsonPropertyName("holes")]
        public List<Hole> Holes { get; set; } = new();
    }
}