namespace LL.Data.Model;

public class WordLink
{
    public int Id {  get; set; }
    
    public int SourceId { get; set; }
    public virtual Word? Source { get; set; }
    
    public int TargetId { get; set; }
    public virtual Word? Target { get; set; }
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual Person? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}