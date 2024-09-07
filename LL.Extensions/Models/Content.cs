using System.Text.Json.Serialization;

namespace LL.Extensions.Models;

public class Content
{
    [JsonPropertyName("parts")]
    public Part[] Parts { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }
}
