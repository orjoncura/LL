using LL.Core.Interfaces;
using LL.Core.Services;

namespace LL.API.Configs
{
    public static class DIConfig
    {
        public static IServiceCollection AddDIConfig(this IServiceCollection services)
        {
            services.AddScoped<ISeminarService, SeminarService>();

            return services;
        }
    }
}
