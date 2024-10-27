using LL.Core.Interfaces.Repositories;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class MessageRepository(AppDBContext db)  : IMessageRepository
{
    public int Insert(string recipientAddress, string content, int? recipientId = null)
    {
        var messageContent = new MessageContent()
        {
            Value = content,
            CreatedDate = DateTimeOffset.Now
        };
        
        db.Add(messageContent);
        db.SaveChanges();
        
        var message = new Message()
        {
            RecipientId = recipientId,
            RecipientAddress = recipientAddress,
            StatusId = 1,
            MessageContentId = messageContent.Id,
            DateSend = DateTimeOffset.Now
        };
        
        db.Add(message);
        db.SaveChanges();
        
        var messageLog = new MessageLog()
        {
            MessageId = message.Id,
            RecipientId = recipientId,
            RecipientAddress = recipientAddress,
            StatusId = 1,
            MessageContentId = messageContent.Id,
            DateSend = DateTimeOffset.Now
        };

        db.Add(messageLog);
        db.SaveChanges();

        return message.Id;
    }
}