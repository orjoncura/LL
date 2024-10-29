namespace LL.Core.Interfaces.Repositories;

public interface INewUserRequestRepository
{
    bool HasReachedLimit(string email, DateTimeOffset date, int attemptsLimit);
    bool IsTokenValid(string token);
    string? GetEmailByToken(string token);
    int Insert(string email, string ip, string token, int loginId);
}