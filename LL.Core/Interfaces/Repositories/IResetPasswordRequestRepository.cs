namespace LL.Core.Interfaces.Repositories;

public interface IResetPasswordRequestRepository
{
    bool HasReachedLimit(int userId, DateTimeOffset date, int attemptsLimit);
    bool IsTokenValid(string token);
    int GetUserIdByToken(string token);
    int Insert(int userId, string ip, string token);
}