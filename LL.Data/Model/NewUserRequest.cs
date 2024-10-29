namespace LL.Data.Model;

public class NewUserRequest
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string IP { get; set; }
    public string Token { get; set; }
    
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }  
}