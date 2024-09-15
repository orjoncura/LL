namespace LL.Data.Model;

public class UserToken 
{
    public int Id { get; set; }
    public Guid Token { get; set; }
    public DateTimeOffset Expiration { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public bool IsActive { get; set; }
}
