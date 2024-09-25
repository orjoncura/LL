namespace LL.Data.Model;

public class UserTokenLog
{
    public long Id { get; set; }

    public int UserTokenId { get; set; }
    public virtual UserToken? UserToken { get; set; } 

    public Guid Token { get; set; }
    public DateTimeOffset Expiration { get; set; }
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

