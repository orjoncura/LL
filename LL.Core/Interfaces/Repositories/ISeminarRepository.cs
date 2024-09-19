namespace LL.Core.Interfaces.Repositories;

public interface ISeminarRepository
{
    int Insert(string value, int fromId, int toId, int userId);
}