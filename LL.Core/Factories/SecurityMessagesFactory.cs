using LL.Core.Constants;

namespace LL.Core.Factories;

public class SecurityMessagesFactory
{
    public static string CreateNewUser(string url, string token, string email)
    {
        string msg = $@"
            Dear User,

            Thank you for registering with us! To complete your registration, please confirm your email address by clicking the link below:

            <a href='{url + Settings.CompleteUserRegistrationUrl(token, email)}'>Confirm Email</a>

            If you did not create an account with us, please ignore this email.
            
            {Settings.Footer}
        ";
        
        return msg;
    }
    public static string ResetPassword(string url, string token, string email)
    {
        string msg = $@"
            Dear User,

            To complete your password reset, please confirm your email address by clicking the link below:

            <a href='{url + Settings.CompleteResetPasswordUrl(token, email)}'>Confirm Email</a>

            If you did not try to reset your password, please ignore this email.
            
            {Settings.Footer}
        ";
        
        return msg;
    }
}