using LL.Core.Enums;
using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class WordViewModel
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public int Importance { get; set; } 
    public List<MeaningShort> Meanings { get; set; } 

    public WordViewModel(WordShort wordShort, List<MeaningShort> meanings, int importance, string translation)
    {
        Id = wordShort.Id;
        Name = wordShort.Name;
        Meanings = meanings;
        Importance = importance;
        Translation = translation;
    }
}