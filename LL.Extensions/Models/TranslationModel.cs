using System.Text.Json.Serialization;

namespace LL.Extensions.Models;

using Newtonsoft.Json;

public class TranslationModel
{
    [JsonProperty("inputs")]
    public TranslationInputs Inputs { get; set; }

    [JsonProperty("response")]
    public TranslationResponse Response { get; set; }
}
