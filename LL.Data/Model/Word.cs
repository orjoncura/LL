namespace LL.Data.Model;

public class Word 
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public int LanguageFromId { get; set; }
    public virtual Language? LanguageFrom { get; set; }

    public int LanguageToId { get; set; }
    public virtual Language? LanguageTo { get; set; }

    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual Person? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

