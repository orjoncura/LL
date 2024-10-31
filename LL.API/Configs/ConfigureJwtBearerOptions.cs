using System.Text;
using LL.Core.Model.DataTransferObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LL.API.Configs
{ 
    public class ConfigureJwtBearerOptions(TokenConfigModel token): IConfigureNamedOptions<JwtBearerOptions>
    {
        public void Configure(string? name, JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = token.Issuer,
                ValidAudience = token.Audience,
                LifetimeValidator = CustomLifetimeValidator,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Key))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var token = context.Request.Headers["Authorization"];
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    Console.WriteLine($"Token: {token}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated: " + context.SecurityToken);
                    return Task.CompletedTask;
                }
            };
        }
            
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