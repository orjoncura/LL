namespace LL.Data.Model;

public class Seminar
{
    public int Id { get; set; }
    
    public string Value { get; set; }
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual Person? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}