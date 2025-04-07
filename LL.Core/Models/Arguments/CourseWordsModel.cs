using LL.Core.Enums;

namespace LL.Core.Models.Arguments;

public class CourseWordsModel
{
    public string Word { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public int Importance { get; set; }

    public bool IsValid => 
        string.IsNullOrWhiteSpace(Word) == false 
        && string.IsNullOrWhiteSpace(Translation) == false 
        && string.IsNullOrWhiteSpace(Definition) == false 
        && string.IsNullOrWhiteSpace(PartOfSpeech) == false
        && Importance > 0;
    
    public CourseWordsModel()
    {
    }
}