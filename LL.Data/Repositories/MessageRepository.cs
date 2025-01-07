using System.Transactions;
using LL.Core.Interfaces.Repositories;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class MessageRepository(AppDbContext db)  : IMessageRepository
{
    public int Insert(string recipientAddress, string subject, string content, int loginId, int? recipientId = null)
    {
        using (var scope = new TransactionScope())
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
                Subject = subject,
                StatusId = 1,
                MessageContentId = messageContent.Id,
                DateSend = null,
            };

            db.Add(message);
            db.SaveChanges();

            var messageLog = new MessageLog()
            {
                MessageId = message.Id,
                RecipientId = recipientId,
                RecipientAddress = recipientAddress,
                Subject = subject,
                StatusId = 1,
                MessageContentId = messageContent.Id,
                DateSend = null,
                CreatedById = loginId,
                CreatedDate = DateTimeOffset.Now
            };

            db.Add(messageLog);
            db.SaveChanges();

            scope.Complete();
            
            return message.Id;
        }
    }
}