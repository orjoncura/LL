namespace LL.Resources.Models;

public class UserLog 
{
    public long Id { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }
    
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}

