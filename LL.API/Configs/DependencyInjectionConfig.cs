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
            services.AddScoped<IWordLinkRepository, WordLinkRepository>();
            services.AddScoped<IWordMeaningRepository, WordMeaningRepository>();
            services.AddScoped<IWordDefinitionRepository, WordDefinitionRepository>();
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<ISeminarWordRepository, SeminarWordRepository>();
            
            //Extensions
            services.AddScoped<IAppMonitoringService, AppMonitoringService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            
            return services;
        }
    }
}
