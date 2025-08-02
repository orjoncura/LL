using LL.Core.Models.Short;

namespace LL.Core.Models.ViewModels;

public class ExerciseViewModel
{
    public string Id { get; set; }
    public string Original { get; set; }
    public string Translated { get; set; }
    public string Extra { get; set; }
    
    public bool IsValid =>
        string.IsNullOrWhiteSpace(Original) == false
        && string.IsNullOrWhiteSpace(Translated) == false
        && string.IsNullOrWhiteSpace(Extra) == false;

    public ExerciseViewModel()
    {
        
    }
}