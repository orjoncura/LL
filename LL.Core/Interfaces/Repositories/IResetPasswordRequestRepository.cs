namespace LL.Core.Interfaces.Repositories;

public interface IResetPasswordRequestRepository
{
    bool HasReachedLimit(int userId, DateTimeOffset date, int attemptsLimit);
    int GetUserIdByToken(string token);
    string Insert(int userId, string ip);
}