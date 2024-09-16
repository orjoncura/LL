using LL.Core.Interfaces;
using LL.Core.Services;
using LL.Data.Interfaces;
using LL.Data.Repositories;

namespace LL.API.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
        {
            services.AddScoped<ISeminarService, SeminarService>();
            services.AddScoped<ISeminarRepository, SeminarRepository>();
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<ISeminarWordRepository, SeminarWordRepository>();
            
            return services;
        }
    }
}
