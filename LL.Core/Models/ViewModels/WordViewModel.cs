using LL.Core.Enums;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class WordViewModel
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public List<MeaningShort> Meanings { get; set; } 
    public int ImportanceRatingId { get; set; }

    public WordViewModel(int id, string name, string translation, int importanceRatingId)
    {
        Id = id;
        Name = name;
        Translation = translation;
        ImportanceRatingId = importanceRatingId;
    }
    public WordViewModel(WordShort wordShort, string translation)
    {
        Id = wordShort.Id;
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