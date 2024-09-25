using System.IdentityModel.Tokens.Jwt;
using LL.Core.Helpers;
using System.Security.Claims;
using System.Text;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModel;
using LL.Core.Models.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LL.Core.Services
{
    public class SecurityService(
        IUserRepository userRepository,
        IOptions<TokenConfigModel> config)
        : ISecurityService
    {
        private readonly TokenConfigModel _tokenConfig = config.Value;

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

        public TokenViewModel GenerateToken(UserShort user)
        {
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_tokenConfig.Expires));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Key ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = GetClaims(user);

            var token = new JwtSecurityToken(
                _tokenConfig.Issuer,
                _tokenConfig.Audience,
                claims,
                expires: expires,
                signingCredentials: credentials);

            return new TokenViewModel()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
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
}
