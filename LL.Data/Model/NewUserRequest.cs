namespace LL.Data.Model;

public class NewUserRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string IP { get; set; }
    public Guid Token { get; set; }
    public DateTimeOffset CreatedDate { get; set; }     
}