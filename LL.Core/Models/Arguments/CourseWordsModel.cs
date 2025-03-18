namespace LL.Core.Models.Arguments;

public class CourseWordsModel
{
    public string Word { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public int Importance { get; set; }

    public CourseWordsModel(string word, string translation, int importance)
    {
        Word = word;
        Translation = translation;
        Importance = importance;
    }
}