namespace LL.Core.Interfaces.Repositories;

public interface IResetPasswordRequestRepository
{
    bool HasReachedLimit(int userId, DateTime date, int attemptsLimit);
    int GetUserIdByToken(string token);
    string Insert(int userId, string ip);
}