using LL.Core.Interfaces;
using LL.Core.Services;
using LL.Data.Interfaces;
using LL.Data.Repositories;

namespace LL.API.Configs
{
    public static class DIConfig
    {
        public static IServiceCollection AddDIConfig(this IServiceCollection services)
        {
            services.AddScoped<ISeminarService, SeminarService>();
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IStatementRepository, StatementRepository>();

            return services;
        }
    }
}
