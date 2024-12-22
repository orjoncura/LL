using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Model.DataTransferObjects;
using LL.Core.Models.DataTransferObjects;
using LL.Core.Services;
using LL.Data.Repositories;
using LL.Extensions;

namespace LL.API.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services, IConfiguration config)
        {
            //Core Services
            services.AddScoped<ISeminarService, SeminarService>();
            services.AddScoped<ISecurityService, SecurityService>();
            
            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISeminarRepository, SeminarRepository>();
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IWordLinkRepository, WordLinkRepository>();
            services.AddScoped<IWordMeaningRepository, WordMeaningRepository>();
            services.AddScoped<IWordDefinitionRepository, WordDefinitionRepository>();
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<ISeminarWordRepository, SeminarWordRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<INewUserRequestRepository, NewUserRequestRepository>();
            services.AddScoped<IResetPasswordRequestRepository, ResetPasswordRequestRepository>();
            
            //Extensions
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAppMonitoringService, AppMonitoringService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<ITextToSpeechService, TextToSpeechService>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped<IEncryptionService, EncryptionService>();

            //Singletons
            services.AddSingleton(
                new TokenConfigModel(
                    config[Secrets.JwtKey],
                config[Secrets.JwtIssuer],
                config[Secrets.JwtAudience],
                config[Secrets.JwtExpiryMinutes]));

            services.AddSingleton(new AgentModel(config[Secrets.GeminiAPI], config[Secrets.LLamaLocation]));
            services.AddSingleton(new StorageModel(config[Secrets.StorageAccessKey], config[Secrets.StorageSecretKey], config[Secrets.StorageName]));
            
            return services;
        }
    }
}
