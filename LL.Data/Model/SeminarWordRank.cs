namespace LL.Data.Model;

public class SeminarWordRank
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual Person? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}