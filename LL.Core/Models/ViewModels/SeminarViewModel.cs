using LL.Core.Enums;
using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class SeminarViewModel
{
    public WordViewModel TargetWord { get; set; } = new WordViewModel();

    public List<StatementViewModel> Sentences { get; set; } = new List<StatementViewModel>();
    
    public bool IsValid => 
        string.IsNullOrWhiteSpace(TargetWord.Name) == false
        && string.IsNullOrWhiteSpace(TargetWord.Definition) == false
        && Enum.IsDefined(typeof(WordTypeEnum), TargetWord.TypeId)
        && Sentences.Any(s => string.IsNullOrWhiteSpace(s.OriginalStatement) == false
                              && string.IsNullOrWhiteSpace(s.TranslatedStatement) == false);
}

