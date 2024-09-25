namespace LL.Data.Model;

public class Language
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
