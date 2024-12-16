using Newtonsoft.Json;

namespace LL.Extensions.Models;

public class TranslationResponse
{
    [JsonProperty("translated_text")]
    public string TranslatedText { get; set; }
}