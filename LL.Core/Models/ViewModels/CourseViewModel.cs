namespace LL.Core.Models.ViewModels;

public class CourseViewModel
{
    public int Id { get; set; } 
    public string Text { get; set; } = string.Empty;
    
    public DateTimeViewModel CreatedDate { get; set; }
    
    public bool IsValid => Id > 0 && string.IsNullOrEmpty(Text) == false;
}

