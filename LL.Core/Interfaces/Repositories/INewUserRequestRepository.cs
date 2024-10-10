namespace LL.Core.Interfaces.Repositories;

public interface INewUserRequestRepository
{
    bool HasReachedLimit(int userId, DateTimeOffset date, int attemptsLimit);
    int Insert(int userId, string ip, Guid token);
}