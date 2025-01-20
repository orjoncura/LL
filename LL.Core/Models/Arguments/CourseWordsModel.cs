namespace LL.Core.Models.Arguments;

public class CourseWordsModel
{
    public string Word { get; set; } = string.Empty;
    
    public int Importance { get; set; }

    public CourseWordsModel(string word, int importance)
    {
        Word = word;
        Importance = importance;
    }
}