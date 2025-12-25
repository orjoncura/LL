namespace LL.Resources.Models;

public class ServerError
{
    public int Id { get; set; }
    public string InnerException { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
    public string InternetProtocol { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}