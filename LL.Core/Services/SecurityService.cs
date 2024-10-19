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

    public bool CreateNewUserRequest(string email, string ip, int attemptsLimit)
    {
        int newUserRequest = 0;
        
        //Check if the email is in a valid format.
        if (TextHelper.IsValidEmail(email))
        {
            int userId = userRepository.Insert(email.ToLower(), string.Empty);

            if (newUserRequestRepository.HasReachedLimit(userId, new DateTimeOffset(), attemptsLimit) == false)
            {
                Guid token = Guid.NewGuid();

                newUserRequest = newUserRequestRepository.Insert(userId, ip, token);
                messageRepository.Insert(userId, SecurityMessagesFactory.CreateSeminarPrompt(token));
            }
        }

        return newUserRequest > 0;
    }

    public bool VerifyUser(string token)
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
}

