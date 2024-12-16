using Newtonsoft.Json;

namespace LL.Extensions.Models;

public class TranslationInputs
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("source_lang")]
    public string SourceLang { get; set; }

    [JsonProperty("target_lang")]
    public string TargetLang { get; set; }
}
