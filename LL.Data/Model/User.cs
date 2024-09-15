namespace LL.Data.Model;

public class User 
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public int PersonId { get; set; }
    public virtual Person Person { get; set; } = new Person();

    public bool IsActive { get; set; }
}


