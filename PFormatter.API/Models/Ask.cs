using System.Text.Json.Serialization;

namespace PFormatter.API.Models
{
    public record AiRequest(string Prompt);

    public class GeminiResponse
    {
        [JsonPropertyName("replay")]
        public string? Replay { get; set; }
    }

    
}
