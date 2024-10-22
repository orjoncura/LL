namespace LL.Core.Interfaces.Repositories;

public interface IMessageRepository
{
    int Insert(string recipientAddress, string content, int? recipientId = null);
}