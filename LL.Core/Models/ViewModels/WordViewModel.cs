using LL.Core.Enums;
using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class WordViewModel
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public byte[] Audio { get; set; } = [];
    public string Language { get; set; }
    public int Importance { get; set; } 
    public List<MeaningShort> Meanings { get; set; } 

    public WordViewModel(WordShort wordShort, List<MeaningShort> meanings, int importance)
    {
        Id = wordShort.Id;
        Name = wordShort.Name;
        Audio = wordShort.Audio;
        Language = wordShort.Language;
        Meanings = meanings;
        Importance = importance;
    }
}