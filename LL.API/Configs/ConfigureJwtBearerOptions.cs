using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LL.Core.Models;

namespace LL.API.Configs
{
        public class ConfigureJwtBearerOptions(IOptions<TokenConfigModel> tokenConfigModel)
        : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly TokenConfigModel _tokenConfigModel = tokenConfigModel.Value;

        public void Configure(string? name, JwtBearerOptions options) => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _tokenConfigModel.Issuer,
            ValidAudience = _tokenConfigModel.Audience,
            LifetimeValidator = CustomLifetimeValidator,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfigModel.Key)),
        };

        public void Configure(JwtBearerOptions options) => Configure(JwtBearerDefaults.AuthenticationScheme, options);

        private static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }

            return false;
        }
    }
}