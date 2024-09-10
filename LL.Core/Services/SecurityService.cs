using LL.Core.Helpers;
using LL.Core.Interfaces;
using LL.Core.Models;
using LL.Data.Interfaces;
using LL.Data.Model;
using LL.SharedDefinitions.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LL.Core.Services
{
    public class SecurityService(
        ISecurityRepository securityRepository,
        IOptions<TokenConfigModel> config)
        : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository = securityRepository;
        private readonly TokenConfigModel _tokenConfig = config.Value;

        public TokenViewModel? Authenticate(LoginModel userLogin)
        {
            var user = _securityRepository.GetLoginByUsername(userLogin.Username);

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

        public TokenViewModel GenerateToken(User user)
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


        private static Claim[] GetClaims(User user)
        {
            return
            [
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Username ?? string.Empty)
            ];
        }
    }
}
