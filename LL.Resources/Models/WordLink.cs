namespace LL.Resources.Models;

public class WordLink
{
    public int Id {  get; set; }
    
    public int WordId { get; set; }
    public virtual Word? Word { get; set; }
    
    public string Value { get; set; } = string.Empty;
    public int LanguageId {  get; set; }
    
    public bool  IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}