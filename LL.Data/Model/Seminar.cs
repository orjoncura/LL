namespace LL.Data.Model;

public class Seminar
{
    public int Id { get; set; }
    public string Value { get; set; }
    
    public int LanguageFromId { get; set; }
    public virtual Language? LanguageFrom { get; set; }

    public int LanguageToId { get; set; }
    public virtual Language? LanguageTo { get; set; }
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}