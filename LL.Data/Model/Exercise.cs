namespace LL.Data.Model;

public class Exercise
{
    public int Id { get; set; }

    public int CourseWordId { get; set; }
    public virtual CourseWord? SeminarWord { get; set; }
    public string Original { get; set; } = string.Empty;
    public string Translated { get; set; } = string.Empty;
    public string Extra { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

