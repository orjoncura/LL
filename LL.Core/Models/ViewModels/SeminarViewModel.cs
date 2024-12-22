using LL.Core.Enums;
using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class SeminarViewModel
{
    public int SeminarId { get; set; } 

    public List<WordViewModel> Words { get; set; } = new List<WordViewModel>();
    
    public bool IsValid => 
        SeminarId > 0
        && Words.Any(w => string.IsNullOrWhiteSpace(w.Name) == false
                          && string.IsNullOrWhiteSpace(w.Name) == false);
}

