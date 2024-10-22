using System.IdentityModel.Tokens.Jwt;
using LL.Core.Helpers;
using System.Security.Claims;
using System.Text;
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

    public bool CreateNewUserRequest(NewUserModel model, string ip, int attemptsLimit)
    {
        model.Email = model.Email.ToLower().Trim();
        model.ConfirmEmail = model.ConfirmEmail.ToLower().Trim();
        
        //Check if the email is in a valid format also check if both emails are the same.
        if (!TextHelper.IsValidEmail(model.Email) || !TextHelper.IsValidEmail(model.ConfirmEmail) || model.Email != model.ConfirmEmail)
            return false;
        
        if (newUserRequestRepository.HasReachedLimit(model.Email, new DateTimeOffset(), attemptsLimit))
            return false;
        
        var token = GenerateToken();

        if (token == null)
            return false;
        
        return newUserRequestRepository.Insert(model.Email, ip, token.Value) > 0 
               && messageRepository.Insert(model.Email, SecurityMessagesFactory.CreateSeminarPrompt(token.Value)) > 0;
    }

    public bool VerifyUser(VerifyUserModel model)
    {
        string email = newUserRequestRepository.GetEmailByToken(model.Token);
        
        if(string.IsNullOrWhiteSpace(email))
            return false;

        return userRepository.Insert(email, model.Password) > 0;
    }

    public bool ResetPassword(string email)
    {
        return false;
    }
    public bool CompletePasswordReset(NewPasswordModel model)
    {
        return false;
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

    private Guid? GenerateToken()
    {
        Guid token = Guid.NewGuid();
        
        for (int i = 0; i < 1000; i++)
        {
            if(newUserRequestRepository.IsTokenValid(token))
                return token;
                    
            token = Guid.NewGuid();
        }

        return null;
    }
}

