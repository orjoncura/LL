namespace LL.Core.Factories;

public class SecurityMessagesFactory
{
    public static string CreateSeminarPrompt(Guid token)
    {
        string msg = string.Format(@"Please confirm your email address {0}", token);

        return msg;
    }
}