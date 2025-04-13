namespace LL.Resources.Models;

public class ResetPasswordRequest
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    
    public string IP { get; set; }
    public string Token { get; set; }
    public DateTimeOffset CreatedDate { get; set; }     
}