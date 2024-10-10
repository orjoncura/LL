namespace LL.Data.Model;

public class LoginHistory 
{
    public int Id { get; set; }

    public string IP  { get; set; } 
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

