using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.ViewModel;
using LL.Core.Services;
using LL.Data.Repositories;
using LL.Extensions;

namespace LL.API.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
        {
            //Core Services
            services.AddScoped<ISeminarService, SeminarService>();
            
            //Repositories
            services.AddScoped<ISeminarRepository, SeminarRepository>();
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<ISeminarWordRepository, SeminarWordRepository>();
            
            //Extensions
            services.AddScoped<IAppMonitoring, AppMonitoring>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<ITranslation, Translation>();
            
            return services;
        }
    }
}
