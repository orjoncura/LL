using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services;

public interface ISecurityService
{
    TokenViewModel? Authenticate(LoginModel loginModel);
    bool RegisterUser(NewUserModel model, string ip, int attemptsLimit, int loginId);
    bool CompleteUserRegistration(ConfirmationModel model, int loginId);
    bool ResetPassword(string email, string ip, int attemptsLimit);
    bool CompletePasswordReset(ConfirmationModel model);
}

