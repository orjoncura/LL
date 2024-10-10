namespace LL.Core.Interfaces.Repositories;

public interface IMessageRepository
{
    int Insert(int recipientId, string contentS);
}