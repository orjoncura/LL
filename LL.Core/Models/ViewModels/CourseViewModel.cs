namespace LL.Core.Models.ViewModels;

public class CourseViewModel
{
    public string Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTimeViewModel CreatedDate { get; set; }
    
    public bool IsValid => Id.Length > 0 && string.IsNullOrEmpty(Text) == false;
}

