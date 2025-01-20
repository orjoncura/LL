using LL.Core.Models.Arguments;

namespace LL.Core.Models.ViewModels;

public class CourseViewModel
{
    public int Id { get; set; } 

    public List<CourseWordsModel> Words { get; set; } = new List<CourseWordsModel>();
    
    public bool IsValid => 
        Id > 0
        && Words.Any(w => string.IsNullOrWhiteSpace(w.Word) == false
                          && string.IsNullOrWhiteSpace(w.Word) == false);
}

