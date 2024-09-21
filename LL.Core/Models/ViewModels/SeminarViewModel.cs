using LL.Core.Enums;
using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class SeminarViewModel
{
    public string TargetWord { get; set; } 

    public List<StatementShort> Sentences { get; set; } 
    
    public int Importance { get; set; } 
    
    public bool IsValid => 
        string.IsNullOrWhiteSpace(TargetWord) == false
        && Sentences.Any(s => string.IsNullOrWhiteSpace(s.OriginalStatement) == false
                              && string.IsNullOrWhiteSpace(s.TranslatedStatement) == false);

    public SeminarViewModel(string word, List<StatementShort> sentences, int importance)
    {
        TargetWord = word;
        Sentences = sentences;
        Importance = importance;
    }
}

