using System.Text.Json.Serialization;

namespace LL.Extensions.Models;

public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}