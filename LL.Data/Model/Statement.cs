namespace LL.Data.Model;

public class Statement
{
    public int Id { get; set; }

    public int SeminarWordId { get; set; }
    public virtual SeminarWord? SeminarWord { get; set; }
    public string Original { get; set; } = string.Empty;
    public string Translated { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

