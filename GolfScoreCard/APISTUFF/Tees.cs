using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GolfScoreCard.APISTUFF
{
    public class Tees
    {
        [JsonPropertyName("female")]
        public List<Tee> Female { get; set; } = new();

        [JsonPropertyName("male")]
        public List<Tee> Male { get; set; } = new();
    }
}