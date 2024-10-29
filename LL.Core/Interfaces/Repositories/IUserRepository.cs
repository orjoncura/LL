using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface IUserRepository
{
    UserShort? GetById(int id);
    UserShort? GetByEmail(string email);
    int Insert(string email, string password,  int loginId);
    bool UpdatePassword(int userId, string password, int loginId);
}

