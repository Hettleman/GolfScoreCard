using System.Text.Json.Serialization;

namespace GolfScoreCard.APISTUFF
{
    public class Hole
    {
        [JsonPropertyName("par")]
        public int Par { get; set; }

        [JsonPropertyName("yardage")]
        public int Yardage { get; set; }

        // Some tees include this, others don’t
        [JsonPropertyName("handicap")]
        public int? Handicap { get; set; }
    }
}