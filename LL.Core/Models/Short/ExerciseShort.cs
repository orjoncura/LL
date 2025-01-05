using LL.Core.Models.ViewModels;

namespace LL.Core.Models.Short;

public class ExerciseShort
{
    public string OriginalStatement { get; set; } = string.Empty;
    public string TranslatedStatement { get; set; } = string.Empty;
    
    public bool IsValid =>
        string.IsNullOrWhiteSpace(OriginalStatement) == false
        && string.IsNullOrWhiteSpace(TranslatedStatement) == false;
    public ExerciseShort() { }
    public ExerciseShort(ExerciseViewModel exercise) 
    {
        OriginalStatement = exercise.Original;
        TranslatedStatement = exercise.Translated;
    }
}

