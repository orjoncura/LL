namespace LL.Resources.Models;

public class MessageStatus
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
}