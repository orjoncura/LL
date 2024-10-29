using System.IdentityModel.Tokens.Jwt;
using LL.Core.Helpers;
using System.Security.Claims;
using System.Text;
using LL.Core.Constants;
using LL.Core.Factories;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModel;
using LL.Core.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace LL.Core.Services;

public class SecurityService(
    IUserRepository userRepository, 
    IMessageRepository messageRepository, 
    INewUserRequestRepository newUserRequestRepository, 
    IResetPasswordRequestRepository resetPasswordRequestRepository,
    TokenConfigModel token) : ISecurityService
{
    public TokenViewModel? Authenticate(LoginModel userLogin)
    {
        var user = userRepository.GetByEmail(userLogin.Email);

        if (user is null)
        {
            return null;
        }

        if (!SecurityHelper.VerifyHashedPassword(user.PasswordHash, userLogin.Password))
        {
            return null;
        }

        return GenerateToken(user);
    }

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
        
        string token = SecurityHelper.GenerateSecureToken();
        
        return newUserRequestRepository.Insert(model.Email, ip, token, Settings.LoginId) > 0 
               && messageRepository.Insert(
                   model.Email, 
                   "Confirm Your Email Address",
                   SecurityMessagesFactory.CreateNewUser(url, token, model.Email), 
                   Settings.LoginId) > 0;
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
            
        return userRepository.Insert(email, SecurityHelper.HashPassword(model.Password), Settings.LoginId) > 0;
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
        
        string token = SecurityHelper.GenerateSecureToken();

        return resetPasswordRequestRepository.Insert(user.Id, ip, token) > 0 
               && messageRepository.Insert(
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
        
        return userRepository.UpdatePassword(userId, SecurityHelper.HashPassword(model.Password), userId);
    }
    
    private TokenViewModel GenerateToken(UserShort user)
    {
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(token.Expires));
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Key ?? string.Empty));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = GetClaims(user);

        var securityToken = new JwtSecurityToken(
            token.Issuer,
            token.Audience,
            claims,
            expires: expires,
            signingCredentials: credentials);

        return new TokenViewModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expires.ToString(),
        };
    }
    private static Claim[] GetClaims(UserShort user)
    {
        return
        [
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Email ?? string.Empty)
        ];
    }
}

