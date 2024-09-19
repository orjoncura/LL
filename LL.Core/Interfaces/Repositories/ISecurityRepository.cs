using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface ISecurityRepository
{
    public UserShort? GetLoginByUsername(string username);
}

