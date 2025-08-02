using LL.Core.Enums;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class WordViewModel
{
    public string Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public int ImportanceRatingId { get; set; }
    public List<MeaningShort> Meanings { get; set; } 

    public WordViewModel(){}
    public WordViewModel(string id, string name, string translation, int importanceRatingId, List<MeaningShort> meanings)
    {
        Id = id;
        Name = name;
        Translation = translation;
        ImportanceRatingId = importanceRatingId;
        Meanings = meanings;
    }
    public WordViewModel(string id, WordShort wordShort, string translation)
    {
        Id = id;
        Name = wordShort.Name;
        Translation = translation;
        ImportanceRatingId = wordShort.ImportanceRatingId;
    }
    public WordViewModel(CourseWordsModel courseWordsModel)
    {
        Name = courseWordsModel.Word;
        Meanings = new List<MeaningShort>()
        {
            new()
            {
                Type = courseWordsModel.PartOfSpeech,
                Definitions = new List<string>()
                {
                    courseWordsModel.Definition
                }
            }
        };
        Translation = courseWordsModel.Translation;
        ImportanceRatingId = courseWordsModel.Importance;
    }
}