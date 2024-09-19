using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services;

public interface ISecurityService
{
    TokenViewModel? Authenticate(LoginModel loginModel);
}

