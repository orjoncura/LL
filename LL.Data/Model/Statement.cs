namespace LL.Data.Model;

public class Statement
{
    public int Id { get; set; }
    public int WordId { get; set; }
    public virtual Word? Word { get; set; }
    
    public int SeminarId { get; set; }
    public virtual Seminar? Seminar { get; set; }

    public string OriginalStatement { get; set; } = string.Empty;
    public string TranslatedStatement { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual Person? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

