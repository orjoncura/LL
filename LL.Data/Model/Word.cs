namespace LL.Data.Model;

public class Word 
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public int LanguageId { get; set; }
    public virtual Language? Language { get; set; }
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual Person? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

