namespace LL.Core.Interfaces.Repositories;

public interface IMessageRepository
{
    int Insert(string recipientAddress, string subject, string content, int loginId, int? recipientId = null);
}