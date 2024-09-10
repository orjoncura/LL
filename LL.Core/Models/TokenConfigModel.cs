namespace LL.Core.Models
{
    public class TokenConfigModel
    {
        public string Expires { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}