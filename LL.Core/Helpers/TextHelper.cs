using System.Text.RegularExpressions;

namespace LL.Core.Helpers;

public static class TextHelper
{
    public static bool IsValidEmail(string email) =>
        Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    
    public static bool IsPasswordValid(string password) => 
        Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?\\""{}|<>]).+$");
}