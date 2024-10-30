namespace LL.Data.Model;

public class UserToken 
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset Expiration { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }
}
