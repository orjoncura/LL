namespace LL.Resources.Models;

public class WordMeaning
{
    public int Id { get; set; }
    
    public int WordId { get; set; }
    public virtual Word? Word { get; set; }
    
    public int TypeId { get; set; }
    public WordType Type { get; set; }    
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public virtual List<WordDefinition> WordDefinitions { get; set; }
}