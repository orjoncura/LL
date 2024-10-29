using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModel;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IUserRepository
{
    UserShort? GetById(int id);
    UserShort? GetByEmail(string email);
    TokenViewModel? GetAuthenticationToken(LoginModel loginModel, TokenConfigModel token);
    int Insert(string email, string password,  int loginId);
    bool UpdatePassword(int userId, string password, int loginId);
}

