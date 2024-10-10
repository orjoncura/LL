using LL.Core.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace LL.API.Configs;

public static class JwtConfig
{
    public static IServiceCollection AddJwtConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<TokenConfigModel>(config.GetSection("Jwt"));

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

