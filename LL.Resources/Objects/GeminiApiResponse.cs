using System.Text.Json.Serialization;

namespace LL.Extensions.Models;

public class GeminiApiResponse
{
    [JsonPropertyName("candidates")]
    public Candidate[]? Candidates { get; set; }

    [JsonPropertyName("promptFeedback")]
    public PromptFeedback? PromptFeedback { get; set; }
}