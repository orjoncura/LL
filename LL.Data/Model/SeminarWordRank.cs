namespace LL.Data.Model;

public class SeminarWordRank
{
    public int Id { get; set; }
    public string Value { get; set; }
    
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}