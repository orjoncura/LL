namespace LL.Core.Constants;

public static class Settings
{
    public const int AttemptsLimit = 3;
    
    public static string CompleteUserRegistrationUrl(string token, string email) =>
        $"/Security/CompleteUserRegistration?token={token}&email={email}";
    
    public static string CompleteResetPasswordUrl(string token, string email) =>
        $"/Security/CompleteResetPassword?token={token}&email={email}";

    public const string Footer = "Best regards";
}