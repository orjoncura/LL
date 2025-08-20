namespace LL.Core.Models.ViewModels;

public class ProfileViewModel
{
    public string Email { get; set; }
    
    public DateTimeViewModel DateCreated { get; set; }

    public ProfileViewModel()
    {
        
    }
    
    public ProfileViewModel(string email, DateTime dateCreated)
    {
        Email =  email;
        DateCreated = new DateTimeViewModel(dateCreated);
    }
}