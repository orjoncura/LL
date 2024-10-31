namespace LL.Core.Model.DataTransferObjects;

public class TokenConfigModel
{
    public string Expires { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    
    public  TokenConfigModel(string key, string issuer, string audience, string expires)
    {
        Key = key;
        Issuer = issuer;
        Audience = audience;
        Expires = expires;
    }
}
