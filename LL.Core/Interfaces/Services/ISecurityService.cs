using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services;

public interface ISecurityService
{
    bool RegisterUser(NewUserModel model, string ip, string url);   
    bool CompleteUserRegistration(ConfirmationModel model);
    bool ResetPassword(string email, string ip, string url);
    bool CompletePasswordReset(ConfirmationModel model);
}

