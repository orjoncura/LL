namespace LL.Core.Interfaces.Repositories;

public interface INewUserRequestRepository
{
    bool HasReachedLimit(string email, DateTimeOffset date, int attemptsLimit);
    string? GetEmailByToken(string token);
    string Insert(string email, string ip, int loginId);
}