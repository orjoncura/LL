using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface IUserRepository
{
    UserShort? GetByEmail(string email);
    int Insert(string email, string password, int personId);
}

