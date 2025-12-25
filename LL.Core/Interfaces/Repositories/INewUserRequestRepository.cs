namespace LL.Core.Interfaces.Repositories;

public interface INewUserRequestRepository
{
    bool HasReachedLimit(string email, DateTime date, int attemptsLimit);
    string? GetEmailByToken(string token);
    string Insert(string email, string ip, int loginId);
}