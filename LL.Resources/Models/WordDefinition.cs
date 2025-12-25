namespace LL.Resources.Models;

public class WordDefinition
{
    public int Id { get; set; }
    
    public string Value { get; set; }
    
    public int WordMeaningId { get; set; }
    public virtual WordMeaning? WordMeaning { get; set; }   
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}