namespace LL.Data.Model;

public class WordType
{
    public int Id {  get; set; }
    public string Value { get; set; } = string.Empty;
    
    public int CreatedById { get; set; }
    public virtual Person? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}