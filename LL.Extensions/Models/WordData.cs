using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Models.Short;

namespace LL.Extensions.Models;

public class WordData
{
    public string word { get; set; }
    public string phonetic { get; set; }
    public List<Phonetic> phonetics { get; set; }
    public List<Meaning> meanings { get; set; }
    public License license { get; set; }
    public List<string> sourceUrls { get; set; }

    public List<MeaningShort> Meanings() =>
        (from m in meanings
        select new MeaningShort
        {
            TypeId = EnumHelper.GetEnumValue(typeof(WordTypeEnum), m.partOfSpeech),
            Definitions = m.definitions.Select(d => d.definition).ToList(),
        }).ToList();
    
    
}