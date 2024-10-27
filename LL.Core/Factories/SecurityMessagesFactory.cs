namespace LL.Core.Factories;

public class SecurityMessagesFactory
{
    public static string CreateNewUser(Guid token)
    {
        string msg = string.Format(@"Please confirm your email address {0}", token);

        return msg;
    }
    public static string ResetPassword(Guid token)
    {
        string msg = string.Format(@"Please confirm your email address {0}", token);

        return msg;
    }
}