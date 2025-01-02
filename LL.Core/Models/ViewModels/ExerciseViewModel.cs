using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class ExerciseViewModel
{
    public string OriginalStatement { get; set; }
    
    public string TranslatedStatement { get; set; }

    public bool IsValid =>
        string.IsNullOrWhiteSpace(OriginalStatement) == false
        && string.IsNullOrWhiteSpace(TranslatedStatement) == false;

    public ExerciseViewModel(ExerciseShort exercise)
    {
        OriginalStatement = exercise.OriginalStatement;
        TranslatedStatement = exercise.TranslatedStatement;
    }
}