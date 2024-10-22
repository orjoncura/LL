using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services;

public interface ISecurityService
{
    TokenViewModel? Authenticate(LoginModel loginModel);
    bool CreateNewUserRequest(NewUserModel model, string ip, int attemptsLimit);
    bool ResetPassword(string email);
    bool VerifyUser(VerifyUserModel model);
    bool CompletePasswordReset(NewPasswordModel model);
}

