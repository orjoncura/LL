namespace LL.Data.Model;

public class MessageLog
{
    public int Id { get; set; }
    
    public int MessageId { get; set; }
    public virtual Message? Message { get; set; }
    
    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }
    
    public int ReceiverId { get; set; }
    public virtual User? Receiver { get; set; }
    
    public int StatusId { get; set; }
    public MessageStatus? Status { get; set; }
    
    public int MessageContentId { get; set; }
    public virtual MessageContent? MessageContent { get; set; }
    
    public DateTimeOffset DateSend { get; set; }
}