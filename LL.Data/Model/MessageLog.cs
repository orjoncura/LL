namespace LL.Data.Model;

public class MessageLog
{
    public int Id { get; set; }
    
    public int MessageId { get; set; }
    public virtual Message? Message { get; set; }
    
    public int RecipientId { get; set; }
    public virtual User? Recipient { get; set; }
    
    public int StatusId { get; set; }
    public MessageStatus? Status { get; set; }
    
    public int MessageContentId { get; set; }
    public virtual MessageContent? MessageContent { get; set; }
    
    public DateTimeOffset DateSend { get; set; }
}