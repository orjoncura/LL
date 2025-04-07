using LL.Core.Enums;

namespace LL.Core.Models.Arguments;

public class DefinitionRequestModel
{
    public string Text { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;    
    public string Definition { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public int LanguageFromId { get; set; }
    public int LanguageToId { get; set; }

    public bool IsValid => 
        string.IsNullOrWhiteSpace(Text) == false 
        && string.IsNullOrWhiteSpace(Translation) == false 
        && string.IsNullOrWhiteSpace(Definition) == false 
        && string.IsNullOrWhiteSpace(PartOfSpeech) == false;
}