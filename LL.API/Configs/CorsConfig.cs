using LL.Core.Constants;
using Microsoft.AspNetCore.DataProtection;

namespace LL.API.Configs
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration config)
        {
            string[] cors = config.GetSection(Secrets.Cors).Get<string[]>() ?? [];
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy => policy.WithOrigins(cors)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            return services;
        }
    }
}
