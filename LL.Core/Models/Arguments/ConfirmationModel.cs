namespace LL.Core.Models.Arguments;

public class ConfirmationModel
{
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
    
    public string Token { get; set; }
}