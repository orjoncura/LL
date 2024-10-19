namespace LL.Data.Model;

public class User 
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public bool IsVerified { get; set; }
    public bool IsActive { get; set; }
}


