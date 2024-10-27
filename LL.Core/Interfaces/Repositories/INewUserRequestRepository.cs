namespace LL.Core.Interfaces.Repositories;

public interface INewUserRequestRepository
{
    bool HasReachedLimit(string email, DateTimeOffset date, int attemptsLimit);
    bool IsTokenValid(Guid token);
    string GetEmailByToken(string token);
    int Insert(string email, string ip, Guid token, int loginId);
}