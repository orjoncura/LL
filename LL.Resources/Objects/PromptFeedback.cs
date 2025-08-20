using System.Text.Json.Serialization;

namespace LL.Extensions.Models;

public class PromptFeedback
{    
    [JsonPropertyName("safetyRatings")]
    public SafetyRating[]? SafetyRatings { get; set; }
}