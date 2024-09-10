using LL.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace LL.API.Configs
{
    public static class JwtConfig
    {

        public static IServiceCollection AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenConfigModel>(configuration.GetSection("Jwt"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            services.AddAuthorization(opts =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

                opts.DefaultPolicy = defaultAuthBuilder.RequireClaim("UserId").Build();
                opts.FallbackPolicy = defaultAuthBuilder.RequireClaim("UserId").Build();
            });

            services.ConfigureOptions<ConfigureJwtBearerOptions>();

            return services;
        }
    }
}
