using LL.Core.Interfaces.Repositories;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class MessageRepository(AppDBContext db)  : IMessageRepository
{
    public int Insert(int recipientId, string content)
    {
        var messageContent = new MessageContent()
        {
            Value = content,
            CreatedById = recipientId,
            CreatedDate = DateTimeOffset.Now
        };
        
        db.Add(messageContent);
        db.SaveChanges();
        
        var message = new Message()
        {
            RecipientId = recipientId,
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
            StatusId = 1,
            MessageContentId = messageContent.Id,
            DateSend = DateTimeOffset.Now
        };

        db.Add(messageLog);
        db.SaveChanges();

        return message.Id;
    }
}