using System.Text.Json.Serialization;

public class SafetyRating
{
    [JsonPropertyName("category")]
    public string? Category { get; set; }
    
    [JsonPropertyName("probability")]
    public string? Probability { get; set; }
}