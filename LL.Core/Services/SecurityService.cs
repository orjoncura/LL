using LL.Core.Helpers;
using LL.Core.Constants;
using LL.Core.Factories;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Enums;

namespace LL.Core.Services;

public class SecurityService(
    IUserRepository userRepository, 
    IMessageRepository messageRepository, 
    INewUserRequestRepository newUserRequestRepository, 
    IResetPasswordRequestRepository resetPasswordRequestRepository) : ISecurityService
{
    public bool RegisterUser(NewUserModel model, string ip, string url)
    {
        model.Email = model.Email.ToLower().Trim();
        model.ConfirmEmail = model.ConfirmEmail.ToLower().Trim();
        
        //Check if the email is in a valid format also check if both emails are the same.
        if (!TextHelper.IsValidEmail(model.Email) || !TextHelper.IsValidEmail(model.ConfirmEmail) || model.Email != model.ConfirmEmail)
            return false;
        
        if(userRepository.GetByEmail(model.Email) != null)            
            return false;
        
        if (newUserRequestRepository.HasReachedLimit(model.Email, new DateTimeOffset(), Settings.AttemptsLimit))
            return false;

        string token = newUserRequestRepository.Insert(model.Email, ip, (int)UserEnum.Admin);

        if (string.IsNullOrWhiteSpace(token))
            return false;
        
        return messageRepository.Insert(
                   model.Email, 
                   "Confirm Your Email Address",
                   SecurityMessagesFactory.CreateNewUser(url, token, model.Email),
                   (int)UserEnum.Admin) > 0;
    }
    public bool CompleteUserRegistration(ConfirmationModel model)
    {
        string? email = newUserRequestRepository.GetEmailByToken(model.Token);
        
        if(string.IsNullOrWhiteSpace(email))
            return false;

        if(userRepository.GetByEmail(email) != null)
            return false;
        
        if (!TextHelper.IsPasswordValid(model.Password) || model.Password != model.ConfirmPassword)
            return false;
        
        if(!TextHelper.IsValidEmail(model.Email) || model.Email != email)
            return false;
            
        return userRepository.Insert(email, model.Password, (int)UserEnum.Admin) > 0;
    }
    public bool ResetPassword(string email, string ip, string url)
    {
        email = email.ToLower().Trim();
        
        //Check if the email is in a valid format also check if both emails are the same.
        if (!TextHelper.IsValidEmail(email))
            return false;
        
        UserShort? user = userRepository.GetByEmail(email);

        if (user is null)
            return false;
        
        if (resetPasswordRequestRepository.HasReachedLimit(user.Id, new DateTimeOffset(), Settings.AttemptsLimit))
            return false;
        
        string token = resetPasswordRequestRepository.Insert(user.Id, ip);

        if (string.IsNullOrWhiteSpace(token))
            return false;
        
        return messageRepository.Insert(
                   email, 
                   "Reset Your Password",
                   SecurityMessagesFactory.ResetPassword(url, token, user.Email),
                   user.Id) > 0;
    }
    public bool CompletePasswordReset(ConfirmationModel model)
    {        
        int userId = resetPasswordRequestRepository.GetUserIdByToken(model.Token);
        
        if(userId == 0)
            return false;
        
        if (!TextHelper.IsPasswordValid(model.Password) || model.Password != model.ConfirmPassword)
            return false;
        
        if(!TextHelper.IsValidEmail(model.Email) || model.Email != userRepository.GetById(userId)?.Email)
            return false;
        
        return userRepository.UpdatePassword(userId, model.Password, userId);
    }
}

